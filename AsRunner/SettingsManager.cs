using Microsoft.Win32;

namespace AsRunner;

/// <summary>
/// Пользовательские настройки приложения в HKCU\Software\AsRunner (per-user,
/// без прав администратора). По образцу <see cref="AutoStartManager"/>.
/// </summary>
internal static class SettingsManager
{
    private const string KeyPath = @"Software\AsRunner";
    private const string IncludePrereleasesValue = "IncludePrereleases";

    /// <summary>
    /// Ловить ли pre-release-версии (беты) при проверке обновлений.
    /// По умолчанию false — только стабильные релизы.
    /// </summary>
    public static bool IncludePrereleases
    {
        get
        {
            using var key = Registry.CurrentUser.OpenSubKey(KeyPath);
            return key?.GetValue(IncludePrereleasesValue) is int v && v != 0;
        }
        set
        {
            using var key = Registry.CurrentUser.CreateSubKey(KeyPath);
            key?.SetValue(IncludePrereleasesValue, value ? 1 : 0, RegistryValueKind.DWord);
        }
    }
}
