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

; Обновление: закрываем запущенный экземпляр сами (см. [Code] PrepareToInstall),
; а не через Restart Manager — RM с end-session не смог штатно завершить процесс
; (иконка пропадала, процесс висел, установщик выдавал «unable to close»).
CloseApplications=no

[Files]
Source: "{#PublishDir}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion

[Run]
; Автозапуск после установки — от имени исходного (не-elevated) пользователя.
; skipifsilent — не запускать при тихой установке (для массового развёртывания).
Filename: "{app}\{#MyAppExeName}"; Flags: nowait skipifsilent runasoriginaluser

[Code]
// Закрываем запущенный AsRunner ДО копирования файлов (обновление).
// Сперва мягко (WM_CLOSE — приложение корректно выходит), затем принудительно,
// если ещё жив. Так процесс гарантированно закрыт, без диалогов RM.
function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  ResultCode: Integer;
begin
  Exec('taskkill.exe', '/IM {#MyAppExeName}', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
  Sleep(1500);
  Exec('taskkill.exe', '/F /IM {#MyAppExeName}', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
  Result := '';
end;
