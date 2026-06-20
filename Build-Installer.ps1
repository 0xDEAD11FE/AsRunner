# Build script for AsRunner Installer
# Этот скрипт автоматически собирает приложение и создает MSI инсталлятор

param(
    [string]$Configuration = "Release",
    [string]$OutputDir = ".\output"
)

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "AsRunner Installer Build" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

# Переходим в корневую директорию проекта
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptPath

# Проверка наличия WiX Toolset
Write-Host "Проверка WiX Toolset..." -ForegroundColor Yellow
$wixInstalled = $null
try {
    $wixInstalled = & dotnet tool list --global | Select-String "wix"
} catch {
    Write-Host "WiX Toolset не найден. Устанавливаем..." -ForegroundColor Yellow
    dotnet tool install --global wix
}

if ($null -eq $wixInstalled) {
    Write-Host "WiX Toolset устанавливается..." -ForegroundColor Yellow
    dotnet tool install --global wix
    Write-Host "✓ WiX Toolset установлен" -ForegroundColor Green
} else {
    Write-Host "✓ WiX Toolset уже установлен" -ForegroundColor Green
}

Write-Host ""

# Очистка старых сборок
Write-Host "Очистка предыдущих сборок..." -ForegroundColor Yellow
dotnet clean "AsRunner\AsRunner.csproj" -c $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Host "⚠ Предупреждение при очистке (игнорируем)" -ForegroundColor Yellow
}
Write-Host "✓ Очистка завершена" -ForegroundColor Green
Write-Host ""

# Сборка основного приложения
Write-Host "Сборка AsRunner..." -ForegroundColor Yellow
dotnet build "AsRunner\AsRunner.csproj" -c $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Ошибка при сборке приложения" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Приложение собрано успешно" -ForegroundColor Green
Write-Host ""

# Сборка инсталлятора
Write-Host "Сборка инсталлятора..." -ForegroundColor Yellow
dotnet build "AsRunner.Installer\AsRunner.Installer.wixproj" -c $Configuration
if ($LASTEXITCODE -ne 0) {
    Write-Host "✗ Ошибка при сборке инсталлятора" -ForegroundColor Red
    exit 1
}
Write-Host "✓ Инсталлятор собран успешно" -ForegroundColor Green
Write-Host ""

# Создание выходной директории
if (-not (Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir | Out-Null
}

# Копирование MSI файла
$msiPath = "AsRunner.Installer\bin\$Configuration\AsRunnerSetup.msi"
if (Test-Path $msiPath) {
    Copy-Item $msiPath -Destination $OutputDir -Force
    Write-Host "✓ MSI файл скопирован в: $OutputDir" -ForegroundColor Green
    
    $finalPath = Join-Path $OutputDir "AsRunnerSetup.msi"
    $absolutePath = Resolve-Path $finalPath
    
    Write-Host ""
    Write-Host "=====================================" -ForegroundColor Cyan
    Write-Host "Сборка завершена успешно!" -ForegroundColor Green
    Write-Host "Инсталлятор: $absolutePath" -ForegroundColor Green
    Write-Host "=====================================" -ForegroundColor Cyan
    
    # Вывод информации о размере файла
    $fileInfo = Get-Item $finalPath
    $sizeInMB = [math]::Round($fileInfo.Length / 1MB, 2)
    Write-Host ""
    Write-Host "Размер файла: $sizeInMB MB" -ForegroundColor Cyan
    Write-Host ""
    
    # Опционально: запуск инсталлятора
    $runInstaller = Read-Host "Запустить инсталлятор сейчас? (y/n)"
    if ($runInstaller -eq 'y' -or $runInstaller -eq 'Y') {
        Write-Host "Запуск инсталлятора..." -ForegroundColor Yellow
        Start-Process -FilePath "msiexec.exe" -ArgumentList "/i `"$absolutePath`" /l*v `"$OutputDir\install.log`"" -Wait
    }
} else {
    Write-Host "✗ MSI файл не найден по пути: $msiPath" -ForegroundColor Red
    exit 1
}
