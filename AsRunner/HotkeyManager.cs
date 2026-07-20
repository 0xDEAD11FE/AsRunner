using ConfigReader.Models;
using WinApiWrapper;

namespace AsRunner;

/// <summary>
/// Глобальные горячие клавиши запуска приложений. Регистрирует комбинации из
/// конфига через RegisterHotKey и по нажатию поднимает <see cref="Activated"/>
/// (кроме случая, когда активно полноэкранное приложение).
/// </summary>
internal sealed class HotkeyManager : IDisposable
{
    /// <summary>Невидимое окно-приёмник WM_HOTKEY.</summary>
    private sealed class MessageWindow : NativeWindow
    {
        private readonly Action<int> _onHotKey;

        public MessageWindow(Action<int> onHotKey)
        {
            _onHotKey = onHotKey;
            CreateHandle(new CreateParams());
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == HotKeys.WmHotKey)
                _onHotKey((int)m.WParam);

            base.WndProc(ref m);
        }
    }

    private readonly MessageWindow _window;
    private readonly Dictionary<int, ApplicationConfig> _byId = new();
    private int _nextId = 1;

    /// <summary>Нажата зарегистрированная комбинация — запускаем это приложение.</summary>
    public event Action<ApplicationConfig>? Activated;

    public HotkeyManager() => _window = new MessageWindow(OnHotKey);

    /// <summary>Снимает старые регистрации и регистрирует хоткеи всех приложений из конфига.</summary>
    public void Register(RootConfig config)
    {
        UnregisterAll();

        foreach (var app in config.Values.SelectMany(list => list))
        {
            if (!Hotkey.TryParse(app.Hotkey, out var keyData))
                continue;

            int id = _nextId++;
            if (HotKeys.Register(_window.Handle, id, Hotkey.Modifiers(keyData), Hotkey.VirtualKey(keyData)))
                _byId[id] = app;
            // Не удалось (занято/зарезервировано) — молча пропускаем.
        }
    }

    public void UnregisterAll()
    {
        foreach (var id in _byId.Keys)
            HotKeys.Unregister(_window.Handle, id);

        _byId.Clear();
        _nextId = 1;
    }

    private void OnHotKey(int id)
    {
        if (!_byId.TryGetValue(id, out var app))
            return;

        // Не мешаем полноэкранным приложениям/презентациям.
        if (Shell.IsFullscreenAppActive())
            return;

        Activated?.Invoke(app);
    }

    public void Dispose()
    {
        UnregisterAll();
        _window.DestroyHandle();
    }
}
