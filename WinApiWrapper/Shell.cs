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
}
