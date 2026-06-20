# AsRunner

Windows-трей-приложение (WinForms, .NET 10) для запуска программ под другими
учётными данными через `CreateProcessWithLogonW` с флагом
`LOGON_NETCREDENTIALS_ONLY` (эквивалент `runas /netonly`). Список приложений
берётся из `Config.json`, пароли хранятся в Windows Credential Manager.

Только Windows. Основная оболочка — PowerShell.

## Сборка и запуск

```powershell
dotnet build AsRunner/AsRunner.slnx -c Release   # или -c Debug
```

- Решение в формате `.slnx`; версии пакетов — через `Directory.Packages.props` (CPM, пакетов пока нет).
- `AsRunner` — `WinExe` (трей, без консоли). Выходной файл: `AsRunner/bin/<Cfg>/net10.0-windows/AsRunner.exe`.
- Запущенный экземпляр один (named mutex). UI — иконка в трее; двойной клик открывает управление учётными данными.

## Архитектура (3 проекта)

- **ConfigReader** (`net10.0`) — чтение `Config.json` → `RootConfig`
  (`Dictionary<string, List<ApplicationConfig>>`), модели в `ConfigReader.Models`.
  Файл читается от `AppContext.BaseDirectory` (копируется в output как PreserveNewest).
- **WinApiWrapper** (`net10.0`) — слой P/Invoke (advapi32/kernel32/shell32/user32):
  `ProcessLauncher` (запуск), `CredentialManager` (Credential Manager),
  `IconExtractor` (иконки exe), `SensitiveData` (затирание пароля). Структуры и
  `Constants` — в `WinApiWrapper.Models`.
- **AsRunner** (`net10.0-windows`, WinForms) — UI: `Program` (точка входа, mutex),
  `AsRunnerContext` (корневой `ApplicationContext`), `MenuBuilder`, `AppLauncher`,
  `FormManager`, формы в `Forms/`.

Поток: `Program` → `AsRunnerContext` читает конфиг и через `MenuBuilder` строит
трей-меню → клик по пункту → `AppLauncher` берёт пароль из Credential Manager
(или показывает форму) → `ProcessLauncher.LaunchNetOnly`.

## Конвенции и ограничения

- **Трей-меню = только запускаемые приложения** (+ разделитель и «Выход»). Всё
  служебное (управление кредами и т.п.) — на формах, не в меню.
- **WinApiWrapper не зависит от System.Drawing**: `IconExtractor` отдаёт нативный
  HICON, конвертацию в `Bitmap` делает потребитель (`MenuBuilder`).
- **Гигиена нативных ресурсов**: закрывать хэндлы процесса/потока после запуска,
  `DestroyIcon` для иконок, `Dispose` для `Bitmap` пунктов меню.
- file-scoped namespaces; nullable + ImplicitUsings включены.
- Нативные константы — во вложенных классах (`Constants.LogonFlags`,
  `Constants.ShellFileInfo`) с оригинальным Win32-именем в комментарии.
- Учётные данные: ключ `Tray:{domain}\{username}`, тип Generic, Persist=LocalMachine.

## TODO / в работе

- WiX-инсталлятор: `Quick-Build.ps1` / `Build-Installer.ps1` ссылаются на
  `AsRunner.Installer/AsRunner.Installer.wixproj`, которого ещё нет.
