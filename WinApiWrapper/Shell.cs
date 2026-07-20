using WinApiWrapper.Models;

namespace WinApiWrapper;

/// <summary>Взаимодействие с оболочкой Windows (Explorer).</summary>
public static class Shell
{
    /// <summary>
    /// Сообщить оболочке, что изменились ассоциации/контекстные меню, чтобы Explorer
    /// сбросил кеш и перечитал их (после правки записей контекстного меню в реестре).
    /// </summary>
    public static void NotifyAssociationsChanged()
        => Methods.SHChangeNotify(
            Constants.ShellChange.AssocChanged,
            Constants.ShellChange.IdList,
            IntPtr.Zero,
            IntPtr.Zero);

    /// <summary>
    /// true, если сейчас активно полноэкранное приложение (эксклюзивный D3D) или режим
    /// презентации — чтобы не срабатывать глобальными хоткеями и не мешать ему.
    /// </summary>
    public static bool IsFullscreenAppActive()
    {
        // S_OK == 0; если запрос не удался — считаем, что мешать некому.
        if (Methods.SHQueryUserNotificationState(out int state) != 0)
            return false;

        return state == Constants.UserNotificationState.RunningD3DFullScreen
            || state == Constants.UserNotificationState.PresentationMode;
    }
}
