# Простой скрипт быстрой сборки инсталлятора

Write-Host "Сборка AsRunner и инсталлятора..." -ForegroundColor Cyan
Write-Host ""

# Определяем корневую директорию проекта
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

# Сборка приложения
Write-Host "[1/2] Сборка приложения..." -ForegroundColor Yellow
dotnet build "AsRunner\AsRunner.csproj" -c Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "Ошибка при сборке приложения!" -ForegroundColor Red
    exit 1
}

# Сборка инсталлятора
Write-Host "[2/2] Сборка инсталлятора..." -ForegroundColor Yellow
dotnet build "AsRunner.Installer\AsRunner.Installer.wixproj" -c Release

if ($LASTEXITCODE -ne 0) {
    Write-Host "Ошибка при сборке инсталлятора!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Готово! MSI файл:" -ForegroundColor Green
$msiPath = "AsRunner.Installer\bin\Release\AsRunnerSetup.msi"
$absoluteMsiPath = Resolve-Path $msiPath
Write-Host "$absoluteMsiPath" -ForegroundColor Cyan

# Показать размер файла
if (Test-Path $msiPath) {
    $size = (Get-Item $msiPath).Length / 1MB
    Write-Host "Размер: $([math]::Round($size, 2)) MB" -ForegroundColor Cyan
}
