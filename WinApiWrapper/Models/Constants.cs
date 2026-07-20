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

    /// <summary>Параметры SHChangeNotify.</summary>
    internal static class ShellChange
    {
        internal const int AssocChanged = 0x08000000; // SHCNE_ASSOCCHANGED
        internal const uint IdList = 0x00000000;      // SHCNF_IDLIST
    }

    /// <summary>QUERY_USER_NOTIFICATION_STATE (SHQueryUserNotificationState).</summary>
    internal static class UserNotificationState
    {
        internal const int NotPresent = 1;           // QUNS_NOT_PRESENT
        internal const int Busy = 2;                 // QUNS_BUSY
        internal const int RunningD3DFullScreen = 3;  // QUNS_RUNNING_D3D_FULL_SCREEN
        internal const int PresentationMode = 4;      // QUNS_PRESENTATION_MODE
        internal const int AcceptsNotifications = 5;  // QUNS_ACCEPTS_NOTIFICATIONS
        internal const int QuietTime = 6;             // QUNS_QUIET_TIME
        internal const int App = 7;                   // QUNS_APP
    }
}
