using System.Globalization;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;

namespace AsRunner;

/// <summary>Инфо о доступном обновлении.</summary>
internal sealed record UpdateInfo(Version Version, string ReleaseUrl, string? InstallerUrl);

/// <summary>
/// Проверка обновлений через публичный GitHub Releases API.
/// Не чаще раза в сутки; любые ошибки (сеть, приватный репозиторий → 404, парсинг)
/// проглатываются — проверка не должна мешать работе приложения.
/// </summary>
internal static class UpdateChecker
{
    private const string LatestReleaseApi =
        "https://api.github.com/repos/0xDEAD11FE/AsRunner/releases/latest";

    // Как часто проверять. Раз в неделю; поставь TimeSpan.FromDays(1) для ежедневной.
    private static readonly TimeSpan MinInterval = TimeSpan.FromDays(7);

    private static string StampPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "AsRunner", "last-update-check");

    public static async Task CheckAsync(Action<UpdateInfo> onUpdateAvailable)
    {
        try
        {
            if (!DueForCheck())
                return;

            var current = Assembly.GetExecutingAssembly().GetName().Version;
            if (current is null)
                return;

            using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
            http.DefaultRequestHeaders.UserAgent.ParseAdd("AsRunner-UpdateCheck");
            http.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");

            var json = await http.GetStringAsync(LatestReleaseApi);
            MarkChecked(); // отметили факт проверки, даже если новее ничего нет

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            var tag = root.TryGetProperty("tag_name", out var t) ? t.GetString() : null;
            var latest = ParseVersion(tag);
            if (latest is null || !IsNewer(latest, current))
                return;

            string releaseUrl = root.TryGetProperty("html_url", out var h) ? h.GetString() ?? "" : "";

            string? installerUrl = null;
            if (root.TryGetProperty("assets", out var assets))
            {
                foreach (var a in assets.EnumerateArray())
                {
                    var name = a.TryGetProperty("name", out var n) ? n.GetString() : null;
                    if (name is not null && name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) &&
                        a.TryGetProperty("browser_download_url", out var u))
                    {
                        installerUrl = u.GetString();
                        break;
                    }
                }
            }

            onUpdateAvailable(new UpdateInfo(latest, releaseUrl, installerUrl));
        }
        catch
        {
            // намеренно молча
        }
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

    private static Version? ParseVersion(string? tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return null;

        // "v1.2.3" / "v1.2.3-beta" -> 1.2.3
        var s = tag.TrimStart('v', 'V').Split('-')[0];
        return Version.TryParse(s, out var v) ? v : null;
    }

    private static bool IsNewer(Version latest, Version current)
    {
        // Сравниваем Major.Minor.Build (revision игнорируем).
        static int N(int x) => x < 0 ? 0 : x;
        var l = new Version(N(latest.Major), N(latest.Minor), N(latest.Build));
        var c = new Version(N(current.Major), N(current.Minor), N(current.Build));
        return l > c;
    }
}
