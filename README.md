# AsRunner

![Platform](https://img.shields.io/badge/platform-Windows%20x64-blue)
![.NET](https://img.shields.io/badge/.NET-10-512BD4)

[Latest release →](https://github.com/0xDEAD11FE/AsRunner/releases/latest)

A lightweight Windows **tray utility for launching applications under different
user credentials** — a network-only logon, equivalent to `runas /netonly`.

Handy when you regularly start tools (SSMS, Visual Studio, Explorer, terminals…)
under a domain account that differs from the one you're logged in with: pick the
app from the tray, and it starts under the chosen account.

## Features

- Launch configured apps straight from the **tray menu** under a chosen account.
- Passwords are kept in **Windows Credential Manager**; you're prompted once if a
  credential isn't saved yet.
- App **icons** in the tray menu, plus an optional **alias** to name entries
  (e.g. tell *SSMS 18* and *SSMS 22* apart). Group apps into submenus.
- Optional per-app **command-line arguments**, with a `{folder}` placeholder.
- **Explorer folder context menu** (opt-in per app): right-click inside a folder
  → *AsRunner* → launch an app there (that folder becomes its working directory).
- **Global hotkeys**: assign a per-app shortcut (e.g. `Ctrl+Alt+K`) to launch it
  from anywhere; the picker shows whether a combo is free, and hotkeys stay quiet
  over fullscreen apps/games.
- **Run at Windows startup** toggle, plus an **automatic update check** with an
  optional beta channel.
- Manage apps, credentials and settings from the **UI**.
- Single instance, minimal footprint.

## Install

1. Download the latest `AsRunnerSetup-x.y.z.exe` from the
   [**Releases**](https://github.com/0xDEAD11FE/AsRunner/releases/latest) page.
2. Run it, choose the install folder, finish (per-machine install, asks for UAC).
   On update a running instance is closed automatically, and AsRunner is launched
   when setup finishes.

**Requirements**

- Windows x64
- [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/10.0)

## Usage

- The installer starts **AsRunner** automatically; it then lives in the system
  tray (no shortcut — it's a background tray app).
- **Right-click** the tray icon → menu of your apps → click one to launch it
  (you'll be asked for the password the first time, then it's remembered).
- **Double-click** the tray icon → management window (tabs **Applications**,
  **Credentials** and **Settings**).
- Assign a **hotkey** or enable the **folder context menu** per app when you add
  or edit it on the *Applications* tab. Autostart, the folder-menu integration,
  the beta update channel and a manual **Check for updates** live on the
  *Settings* tab.

## Configuration

The app list lives in `Config.json`. It's resolved in this order:

1. `Config.json` next to the executable — *portable mode* (takes precedence).
2. Otherwise `%APPDATA%\AsRunner\Config.json` — created automatically on first run.

You normally edit it from the UI, but the format is simple — groups of apps:

```json
{
  "Work": [
    {
      "FilePath": "C:\\Program Files\\Microsoft SQL Server Management Studio 22\\Release\\Common7\\IDE\\SSMS.exe",
      "UserName": "your.name",
      "Domain": "domain",
      "Alias": "SSMS 22",
      "Hotkey": "Ctrl+Alt+S"
    },
    {
      "FilePath": "C:\\Windows\\explorer.exe",
      "Arguments": "{folder}",
      "ShowInFolderMenu": true
    }
  ]
}
```

- A single group → flat menu; multiple groups → submenus.
- `UserName` / `Domain` are optional; without them you're prompted at launch.
- `Alias` overrides the menu label (defaults to the file name).
- `Arguments` — optional command-line arguments; `{folder}` is replaced with the
  folder path when the app is launched from the Explorer folder context menu.
- `ShowInFolderMenu` — include this app in the folder context menu (default `false`).
- `Hotkey` — global launch shortcut, e.g. `Ctrl+Alt+S` (default: none).

## Build from source

Requires the **.NET 10 SDK** on Windows.

```powershell
# application
dotnet build AsRunner/AsRunner.slnx -c Release

# installer (Inno Setup 6 must be installed locally)
dotnet publish AsRunner/AsRunner.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o AsRunner/bin/Release/publish
iscc AsRunner.Installer/AsRunner.iss
```

Releases are produced automatically by GitHub Actions on a `v*` tag
(see [`.github/workflows/release.yml`](.github/workflows/release.yml)).

### Project layout

| Project | Target | Role |
|---|---|---|
| `AsRunner` | `net10.0-windows` | WinForms tray app (UI, menu, launch orchestration) |
| `WinApiWrapper` | `net10.0` | P/Invoke layer: process launch, Credential Manager, icons, global hotkeys, shell helpers |
| `ConfigReader` | `net10.0` | reads/writes `Config.json` |
| `AsRunner.Installer/AsRunner.iss` | Inno Setup 6 | installer script |

## Disclaimer

Provided "as is", without warranty of any kind. Use at your own risk.
