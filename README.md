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
  (e.g. tell *SSMS 18* and *SSMS 22* apart).
- Group apps into submenus.
- Manage apps and credentials from the **UI** — add / edit / delete, browse for
  the executable, pick or add an account.
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
- **Double-click** the tray icon → management window (tabs **Applications** and
  **Credentials**).

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
      "Alias": "SSMS 22"
    },
    {
      "FilePath": "C:\\Windows\\explorer.exe"
    }
  ]
}
```

- A single group → flat menu; multiple groups → submenus.
- `UserName` / `Domain` are optional; without them you're prompted at launch.
- `Alias` overrides the menu label (defaults to the file name).

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
| `WinApiWrapper` | `net10.0` | P/Invoke layer: process launch, Credential Manager, icons |
| `ConfigReader` | `net10.0` | reads/writes `Config.json` |
| `AsRunner.Installer/AsRunner.iss` | Inno Setup 6 | installer script |

## Disclaimer

Provided "as is", without warranty of any kind. Use at your own risk.
