using System.Globalization;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace AsRunner;

/// <summary>Инфо о доступном обновлении.</summary>
/// <param name="Version">Числовая версия (Major.Minor.Patch) — напр. для имени файла.</param>
/// <param name="DisplayVersion">Как показывать пользователю: тег без ведущего 'v' (напр. «1.2.0-beta.1»).</param>
internal sealed record UpdateInfo(Version Version, string DisplayVersion, string ReleaseUrl, string? InstallerUrl);

/// <summary>Итог явной проверки обновлений (кнопка «Проверить»).</summary>
internal enum UpdateCheckResult
{
    UpToDate,
    UpdateAvailable,
    Failed,
}

/// <summary>
/// Проверка обновлений через публичный GitHub Releases API.
/// Берём список релизов (а не /releases/latest — тот игнорирует pre-release'ы)
/// и выбираем самый свежий по SemVer с учётом настройки «получать беты».
/// Фоновая проверка — не чаще заданного интервала; ошибки проглатываются.
/// </summary>
internal static class UpdateChecker
{
    // Список релизов: /releases/latest не отдаёт draft/prerelease, поэтому нам не подходит.
    private const string ReleasesApi =
        "https://api.github.com/repos/0xDEAD11FE/AsRunner/releases?per_page=30";

    // Как часто проверять в фоне. Раз в двое суток.
    private static readonly TimeSpan MinInterval = TimeSpan.FromDays(2);

    private static string StampPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AsRunner", "last-update-check");

    /// <summary>Фоновая проверка при старте: не чаще интервала, молча, колбэк только если есть что новее.</summary>
    public static async Task CheckAsync(Action<UpdateInfo> onUpdateAvailable, bool includePrereleases)
    {
        try
        {
            if (!DueForCheck())
                return;

            var current = CurrentVersion();
            if (current is null)
                return;

            var best = await FetchBestAsync(includePrereleases);
            MarkChecked(); // отметили факт проверки, даже если новее ничего нет

            if (best is null || !IsNewer(best.DisplayVersion, current))
                return;

            onUpdateAvailable(best);
        }
        catch
        {
            // намеренно молча
        }
    }

    /// <summary>Явная проверка по кнопке: игнорирует интервал, возвращает результат для показа пользователю.</summary>
    public static async Task<(UpdateCheckResult Result, UpdateInfo? Info)> CheckNowAsync(bool includePrereleases)
    {
        try
        {
            var current = CurrentVersion();
            if (current is null)
                return (UpdateCheckResult.Failed, null);

            var best = await FetchBestAsync(includePrereleases);
            MarkChecked();

            if (best is not null && IsNewer(best.DisplayVersion, current))
                return (UpdateCheckResult.UpdateAvailable, best);

            // Список получили, но новее ничего нет (или подходящих релизов нет) — актуальная версия.
            return (UpdateCheckResult.UpToDate, null);
        }
        catch
        {
            // Сеть/парсинг упали — не смогли проверить.
            return (UpdateCheckResult.Failed, null);
        }
    }

    /// <summary>Тянет список релизов и выбирает самый свежий подходящий. Бросает при сетевых ошибках.</summary>
    private static async Task<UpdateInfo?> FetchBestAsync(bool includePrereleases)
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        http.DefaultRequestHeaders.UserAgent.ParseAdd("AsRunner-UpdateCheck");
        http.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");

        var json = await http.GetStringAsync(ReleasesApi);

        using var doc = JsonDocument.Parse(json);
        if (doc.RootElement.ValueKind != JsonValueKind.Array)
            return null;

        SemVer? bestSem = null;
        string bestDisplay = "";
        string bestReleaseUrl = "";
        string? bestInstallerUrl = null;

        // Список приходит от новых к старым; берём наибольшую версию (по SemVer) среди подходящих.
        foreach (var rel in doc.RootElement.EnumerateArray())
        {
            if (rel.TryGetProperty("draft", out var d) && d.GetBoolean())
                continue;
            if (!includePrereleases && rel.TryGetProperty("prerelease", out var p) && p.GetBoolean())
                continue;

            var tag = rel.TryGetProperty("tag_name", out var t) ? t.GetString() : null;
            var sem = SemVer.Parse(tag);
            if (sem is null)
                continue;

            if (bestSem is not null && sem.CompareTo(bestSem) <= 0)
                continue;

            bestSem = sem;
            bestDisplay = (tag ?? "").TrimStart('v', 'V');   // «v1.2.0-beta.1» -> «1.2.0-beta.1»
            bestReleaseUrl = rel.TryGetProperty("html_url", out var h) ? h.GetString() ?? "" : "";
            bestInstallerUrl = FindInstallerUrl(rel);
        }

        return bestSem is null ? null : new UpdateInfo(bestSem.Numeric, bestDisplay, bestReleaseUrl, bestInstallerUrl);
    }

    private static string? FindInstallerUrl(JsonElement release)
    {
        if (!release.TryGetProperty("assets", out var assets) || assets.ValueKind != JsonValueKind.Array)
            return null;

        foreach (var a in assets.EnumerateArray())
        {
            var name = a.TryGetProperty("name", out var n) ? n.GetString() : null;
            if (name is not null && name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) &&
                a.TryGetProperty("browser_download_url", out var u))
            {
                return u.GetString();
            }
        }
        return null;
    }

    /// <summary>Полная версия текущей сборки (с суффиксом): из AssemblyInformationalVersion.</summary>
    private static SemVer? CurrentVersion()
    {
        var asm = Assembly.GetExecutingAssembly();

        // InformationalVersion хранит полный semver «1.2.0-beta.1» (AssemblyVersion суффикс теряет).
        var info = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        var sem = SemVer.Parse(info);
        if (sem is not null)
            return sem;

        // Фолбэк на числовую версию сборки.
        var v = asm.GetName().Version;
        return v is null ? null : new SemVer(v.Major, Math.Max(0, v.Minor), Math.Max(0, v.Build), []);
    }

    private static bool IsNewer(string candidateDisplay, SemVer current)
    {
        var candidate = SemVer.Parse(candidateDisplay);
        return candidate is not null && candidate.CompareTo(current) > 0;
    }

    private static bool DueForCheck()
    {
        try
        {
            if (File.Exists(StampPath) &&
                DateTime.TryParse(File.ReadAllText(StampPath), CultureInfo.InvariantCulture,
                    DateTimeStyles.RoundtripKind, out var last))
            {
                return DateTime.UtcNow - last >= MinInterval;
            }
        }
        catch { }
        return true;
    }

    private static void MarkChecked()
    {
        try
        {
            var dir = Path.GetDirectoryName(StampPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllText(StampPath, DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));
        }
        catch { }
    }
}
