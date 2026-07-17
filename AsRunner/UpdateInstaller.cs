using System.Diagnostics;
using System.Net.Http;

namespace AsRunner;

/// <summary>
/// Скачивание и запуск установщика обновления. Общий путь и для тоста в трее,
/// и для кнопки «Проверить обновления» в окне управления.
/// </summary>
internal static class UpdateInstaller
{
    public static async Task PromptAndInstallAsync(UpdateInfo info, IWin32Window? owner)
    {
        // Нет .exe в ассетах — просто открываем страницу релиза.
        if (string.IsNullOrEmpty(info.InstallerUrl))
        {
            if (!string.IsNullOrEmpty(info.ReleaseUrl))
                Process.Start(new ProcessStartInfo(info.ReleaseUrl) { UseShellExecute = true });
            return;
        }

        var confirm = MessageBox.Show(owner,
            $"Обновить AsRunner до версии {info.DisplayVersion}?\nПриложение закроется и переустановится.",
            "Обновление AsRunner", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirm != DialogResult.Yes)
            return;

        try
        {
            var temp = Path.Combine(Path.GetTempPath(), $"AsRunnerSetup-{info.Version}.exe");
            using (var http = new HttpClient { Timeout = TimeSpan.FromMinutes(5) })
            {
                http.DefaultRequestHeaders.UserAgent.ParseAdd("AsRunner-UpdateCheck");
                var bytes = await http.GetByteArrayAsync(info.InstallerUrl);
                await File.WriteAllBytesAsync(temp, bytes);
            }

            // Запускаем установщик (ShellExecute → UAC). Он сам закроет это приложение и обновит.
            Process.Start(new ProcessStartInfo(temp) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show(owner, $"Не удалось загрузить обновление:\n{ex.Message}", "Ошибка",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
