namespace WinApiWrapper.Models;

internal static class Constants
{
    /// <summary>Флаги dwLogonFlags для CreateProcessWithLogonW.</summary>
    internal static class LogonFlags
    {
        // Эквивалент /netonly
        internal const uint NetCredentialsOnly = 0x00000002; // LOGON_NETCREDENTIALS_ONLY
    }

    /// <summary>Флаги uFlags для SHGetFileInfo.</summary>
    internal static class ShellFileInfo
    {
        internal const uint Icon = 0x00000100;       // SHGFI_ICON
        internal const uint SmallIcon = 0x00000001;  // SHGFI_SMALLICON
        internal const uint LargeIcon = 0x00000000;  // SHGFI_LARGEICON
    }
}
