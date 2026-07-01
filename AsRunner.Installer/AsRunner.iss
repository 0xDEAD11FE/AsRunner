; Inno Setup script for AsRunner
; Local build:  iscc AsRunner.Installer\AsRunner.iss   (requires the published app, see PublishDir)
; CI passes the version:  iscc /DMyAppVersion=1.2.3 ...

#define MyAppName "AsRunner"
#define MyAppExeName "AsRunner.exe"

#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif

; Папка с опубликованным приложением (single-file exe). Можно переопределить: /DPublishDir=...
#ifndef PublishDir
  #define PublishDir "..\AsRunner\bin\Release\publish"
#endif

[Setup]
AppId={{DFC17B64-2332-4666-B47F-E48B5E0C9AFB}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppName}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
PrivilegesRequired=admin
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
OutputDir=bin
OutputBaseFilename=AsRunnerSetup
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
LicenseFile=License.txt

; Обновление: закрыть запущенный экземпляр автоматически.
; AppMutex — наш named-mutex из Program.cs (детект запущенного приложения).
; CloseApplications+Restart Manager — авто-закрытие процессов, держащих файлы.
AppMutex=AsRunner_SingleInstance_Mutex_24833BA9-80A9-4B40-8B90-5D3E40F4E957
CloseApplications=yes
RestartApplications=no

[Files]
Source: "{#PublishDir}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion

[Run]
; Автозапуск после установки — от имени исходного (не-elevated) пользователя.
; skipifsilent — не запускать при тихой установке (для массового развёртывания).
Filename: "{app}\{#MyAppExeName}"; Flags: nowait skipifsilent runasoriginaluser
