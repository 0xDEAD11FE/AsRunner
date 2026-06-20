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
- Запущенный экземпляр один (named mutex). UI — иконка в трее; двойной клик открывает окно управления (вкладки «Приложения» и «Учётные данные»).

## Архитектура (3 проекта)

- **ConfigReader** (`net10.0`) — чтение/запись `Config.json` → `RootConfig`
  (`Dictionary<string, List<ApplicationConfig>>`), модели в `ConfigReader.Models`.
  Резолв пути: `Config.json` рядом с exe (портативный режим) имеет приоритет,
  иначе — `%APPDATA%\AsRunner\Config.json` (создаётся при первом запуске).
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

## Инсталлятор (WiX)

Проект `AsRunner.Installer/` (WiX v6, SDK-style), сосед основных проектов.
Сборка — правый клик в Visual Studio (нужно расширение HeatWave) или из CLI:

```powershell
dotnet build AsRunner.Installer/AsRunner.Installer.wixproj -c Release
```

Перед сборкой MSI приложение публикуется автоматически (таргет `PublishMainApp`):
single-file, framework-dependent → один `AsRunner.exe`. Результат:
`AsRunner.Installer/bin/x64/Release/AsRunnerSetup.msi`.

Установка per-machine в Program Files (x64), страница выбора папки
(`WixUI_InstallDir`), ярлык в «Пуск», короткий дисклеймер (`License.rtf`).
`Config.json` в MSI не входит (`CopyToPublishDirectory=Never`) — создаётся в
`%APPDATA%`. Инсталлятор x64-only и НЕ собирается в «Build Solution» (Any CPU) —
собирать проект отдельно.

## TODO

- Тоггл автозапуска в приложении (`HKCU\...\Run`, без админа) — автозапуск
  убран из установщика и должен управляться из UI приложения.
