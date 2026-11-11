@echo off
:: 切换控制台为 UTF-8 编码，解决中文乱码
chcp 65001 >nul 2>&1
setlocal enabledelayedexpansion

REM ==========================================
REM 相对路径配置（无需修改）
REM ==========================================
set "PROJECT_DIR=%~dp0"
set "CLASSISLAND_DIR=%~dp0..\ClassIsland"
set "DEPLOY_SCRIPT=%~dp0deploy.ps1"

REM 自动检测 ClassIsland 可执行文件（兼容新旧版本目录结构）
set "CLASSISLAND_EXE=%CLASSISLAND_DIR%\ClassIsland.exe"
if not exist "%CLASSISLAND_EXE%" set "CLASSISLAND_EXE=%CLASSISLAND_DIR%\ClassIsland_App\ClassIsland.Desktop.exe"
if not exist "%CLASSISLAND_EXE%" (
    for /d %%d in ("%CLASSISLAND_DIR%\app-*") do (
        if exist "%%d\ClassIsland.Desktop.exe" set "CLASSISLAND_EXE=%%d\ClassIsland.Desktop.exe"
    )
)

echo =============================================
echo   EntertainingIsland 开发环境一键部署
echo =============================================
echo.
echo   项目目录:     %PROJECT_DIR%
echo   ClassIsland:  %CLASSISLAND_DIR%
echo   ClassIsland 程序: %CLASSISLAND_EXE%
echo.

REM ==========================================
REM 步骤 1: 安装 .NET 8.0 SDK
REM ==========================================
echo [1/4] 检查 .NET SDK...

WHERE dotnet >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [错误] 未找到 dotnet，请先安装 .NET 8.0 SDK
    echo 下载地址: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

REM 检查 SDK 版本
dotnet --list-sdks 2>nul | findstr "8." >nul
if %ERRORLEVEL% NEQ 0 (
    echo .NET 8.0 SDK 未安装，正在下载安装...
    echo.
    
    REM 下载 dotnet-install 脚本
    powershell -ExecutionPolicy Bypass -Command "Write-Host '下载安装脚本...'; Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -OutFile '%TEMP%\dotnet-install.ps1' -UseBasicParsing"
    if %ERRORLEVEL% NEQ 0 (
        echo [错误] 下载失败，请手动安装 .NET 8.0 SDK
        echo 下载地址: https://dotnet.microsoft.com/download/dotnet/8.0
        pause
        exit /b 1
    )
    
    REM 运行安装脚本
    powershell -ExecutionPolicy Bypass -File "%TEMP%\dotnet-install.ps1" -Channel 8.0 -InstallDir "C:\Program Files\dotnet"
    if %ERRORLEVEL% NEQ 0 (
        echo [错误] 安装失败，请手动安装 .NET 8.0 SDK
        pause
        exit /b 1
    )
    
    echo .NET 8.0 SDK 安装完成！
) else (
    echo .NET 8.0 SDK 已安装。
)

REM 确保 dotnet 在 PATH 中
set "PATH=C:\Program Files\dotnet;%PATH%"

REM ==========================================
REM 步骤 2: 下载 ClassIsland（如果不存在）
REM ==========================================
echo.
echo [2/4] 检查 ClassIsland...

if exist "%CLASSISLAND_EXE%" (
    echo ClassIsland 已存在，跳过下载。
) else (
    echo ClassIsland 未找到，正在从 GitHub 下载最新版本...
    echo.
    
    powershell -ExecutionPolicy Bypass -Command ^
      "$ErrorActionPreference = 'Stop';" ^
      "$release = Invoke-RestMethod -Uri 'https://api.github.com/repos/ClassIsland/ClassIsland/releases/latest' -Headers @{'User-Agent'='EntertainingIsland-Setup'} -TimeoutSec 30;" ^
      "$asset = $release.assets | Where-Object { $_.name -like '*ClassIsland*.zip' -or $_.name -like 'ClassIsland_*.zip' } | Select-Object -First 1;" ^
      "if (-not $asset) { $asset = $release.assets | Where-Object { $_.content_type -eq 'application/zip' -or $_.content_type -eq 'application/x-zip-compressed' } | Select-Object -First 1 };" ^
      "if (-not $asset) { Write-Host '错误: 找不到 ClassIsland 下载资源！' -ForegroundColor Red; exit 1 };" ^
      "Write-Host ('  版本: ' + $release.tag_name);" ^
      "Write-Host ('  文件: ' + $asset.name + ' (' + [math]::Round($asset.size/1MB, 1) + ' MB)');" ^
      "Write-Host '  正在下载...';" ^
      "Invoke-WebRequest -Uri $asset.browser_download_url -OutFile '%TEMP%\ClassIsland.zip';" ^
      "Write-Host '  正在解压...';" ^
      "Expand-Archive -Path '%TEMP%\ClassIsland.zip' -DestinationPath '%CLASSISLAND_DIR%' -Force;" ^
      "Remove-Item '%TEMP%\ClassIsland.zip';" ^
      "Write-Host '  ClassIsland 下载完成！' -ForegroundColor Green"
    
    if %ERRORLEVEL% NEQ 0 (
        echo.
        echo [错误] ClassIsland 下载失败！
        echo 请手动下载并解压到: %CLASSISLAND_DIR%
        echo 下载地址: https://github.com/ClassIsland/ClassIsland/releases/latest
        pause
        exit /b 1
    )
    
    REM 检查解压后的 exe 是否真的存在
    if not exist "%CLASSISLAND_EXE%" (
        echo.
        echo [警告] 解压后未在预期位置找到 ClassIsland.Desktop.exe
        echo 预期位置: %CLASSISLAND_EXE%
        echo 请检查解压内容，必要时手动调整目录结构。
        pause
    )
)

REM ==========================================
REM 步骤 3: 构建插件
REM ==========================================
echo.
echo [3/4] 构建插件...
cd /d "%PROJECT_DIR%"

dotnet restore
if %ERRORLEVEL% NEQ 0 (
    echo [错误] 包还原失败！
    pause
    exit /b 1
)

dotnet build -c Debug
if %ERRORLEVEL% NEQ 0 (
    echo [错误] 构建失败！
    pause
    exit /b 1
)

REM ==========================================
REM 步骤 4: 部署插件到 ClassIsland
REM ==========================================
echo.
echo [4/4] 部署插件到 ClassIsland...
powershell -ExecutionPolicy Bypass -File "%DEPLOY_SCRIPT%"
if %ERRORLEVEL% NEQ 0 (
    echo [错误] 部署失败！
    pause
    exit /b 1
)

echo.
echo =============================================
echo   全部完成！
echo.
echo   启动 ClassIsland:
echo   %CLASSISLAND_EXE%
echo.
echo   或者运行:
echo   powershell -ExecutionPolicy Bypass -File "%DEPLOY_SCRIPT%"
echo =============================================
pause
