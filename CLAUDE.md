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
- Запущенный экземпляр один (named mutex). UI — иконка в трее; двойной клик открывает окно управления (вкладки «Приложения», «Учётные данные», «Настройки»).

## Архитектура (3 проекта)

- **ConfigReader** (`net10.0`) — чтение/запись `Config.json` → `RootConfig`
  (`Dictionary<string, List<ApplicationConfig>>`), модели в `ConfigReader.Models`.
  Резолв пути: `Config.json` рядом с exe (портативный режим) имеет приоритет,
  иначе — `%APPDATA%\AsRunner\Config.json` (создаётся при первом запуске).
- **WinApiWrapper** (`net10.0`) — слой P/Invoke (advapi32/kernel32/shell32/user32):
  `ProcessLauncher` (запуск), `CredentialManager` (Credential Manager),
  `IconExtractor` (иконки exe), `SensitiveData` (затирание пароля),
  `Shell` (`SHChangeNotify` для меню папок; `SHQueryUserNotificationState` —
  детект полноэкранного режима), `HotKeys` (`RegisterHotKey`/`UnregisterHotKey`
  + проба занятости). Структуры и `Constants` — в `WinApiWrapper.Models`.
- **AsRunner** (`net10.0-windows`, WinForms) — UI и оркестрация: `Program`
  (точка входа, mutex; ещё режим `--folder-run` — запуск из меню папок),
  `AsRunnerContext` (корневой `ApplicationContext`), `MenuBuilder`, `AppLauncher`,
  `FormManager`, формы в `Forms/` (`MainForm` — трей + тост обновления;
  `ManagementForm` — вкладки; редактор приложения; форма ввода пароля).
  Функциональные модули: `FolderContextMenu` (подменю в «пустом месте» папки
  Explorer), `HotkeyManager` + `Hotkey` (глобальные хоткеи запуска),
  `UpdateChecker` + `UpdateInstaller` + `SemVer` (обновления с GitHub Releases),
  `SettingsManager` (настройки в HKCU), `AutoStartManager` (автозапуск HKCU\...\Run).

Модель `ApplicationConfig`: `FilePath` (+ опц. `UserName`/`Domain`/`Alias`/
`Arguments`/`ShowInFolderMenu`/`Hotkey`). `{folder}` в `Arguments` заменяется путём
папки при запуске из меню папок.

Поток: `Program` → `AsRunnerContext` читает конфиг и через `MenuBuilder` строит
трей-меню → клик по пункту → `AppLauncher` берёт пароль из Credential Manager
(или показывает форму) → `ProcessLauncher.LaunchNetOnly`. Дополнительно
`AsRunnerContext` при старте регистрирует хоткеи (`HotkeyManager`) и через пару
секунд запускает фоновую проверку обновлений (`UpdateChecker`).

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
- Горячие клавиши: `HotKeys.Register` с `MOD_NOREPEAT`; при открытом окне
  управления `AsRunnerContext` временно снимает свои хоткеи, чтобы проба
  «свободна/занята» в редакторе не считала их занятыми.
- Меню папок строится только из приложений с `ShowInFolderMenu`
  (`HKCU\Software\Classes\Directory\Background\shell\AsRunner`), пустые группы
  отбрасываются; глобальный тумблер интеграции — на вкладке «Настройки».

## Обновления

`UpdateChecker` берёт список `/releases` (не `/releases/latest` — тот прячет
pre-release) и выбирает самый свежий по SemVer (`SemVer.cs`). Текущая версия — из
`AssemblyInformationalVersion` (там сохраняется суффикс `-beta.N`; в CI полный тег
идёт в `-p:Version`, а `IncludeSourceRevisionInInformationalVersion=false` убирает
`+<hash>`). Канал бет — тумблер в «Настройках» (HKCU, `SettingsManager`); фоновая
проверка при старте раз в 2 суток, кнопка «Проверить обновления» игнорирует интервал.
Версионирование: тег `vX.Y.Z[-beta.N]`, стабильный релиз старше любой своей беты.

## Инсталлятор (Inno Setup)

Скрипт `AsRunner.Installer/AsRunner.iss` (Inno Setup 6). Сборка — сперва publish
приложения (single-file, framework-dependent), затем `iscc` (локально нужен
установленный Inno Setup 6):

```powershell
dotnet publish AsRunner/AsRunner.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o AsRunner/bin/Release/publish
iscc AsRunner.Installer/AsRunner.iss   # /DMyAppVersion=1.2.3 — версия из CI
```

Результат: `AsRunner.Installer/bin/AsRunnerSetup.exe`.

Установка per-machine в Program Files (x64), страница выбора папки, короткий
дисклеймер (`License.txt`), **без ярлыков**. При обновлении запущенный экземпляр
закрывается автоматически (`AppMutex` = named-mutex из `Program.cs` +
Restart Manager). После установки приложение стартует от имени пользователя
(`runasoriginaluser`, не elevated). `Config.json` в пакет не входит
(`CopyToPublishDirectory=Never`) — создаётся в `%APPDATA%`.

Релизы собирает GitHub Actions по тегу `v*` (`.github/workflows/release.yml`):
publish → установка Inno через choco → `iscc` → черновик Release через `gh` CLI.
В сборку идёт полная версия тега (`-p:Version`), в имя ассета — тоже полная
(`AsRunnerSetup-1.2.0-beta.1.exe`), Inno берёт числовую часть (`X.Y.Z`).

## TODO

- Запуск по горячей клавише в **активной папке Проводника** (COM
  `Shell.Application` → путь окна) — отложено (см. заметку памяти
  `hotkey-folder-detection`).
