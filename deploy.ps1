# ClassIsland 插件部署和调试脚本
# 用法: .\deploy.ps1 [-Debug]

param([switch]$Debug)

$ErrorActionPreference = 'Stop'

# ==========================================
# 相对路径配置（无需修改）
# ==========================================
$pluginProject    = $PSScriptRoot
$pluginOutput     = "$pluginProject\bin\Debug"
$classIslandRoot  = "$PSScriptRoot\..\ClassIsland"                              # ClassIsland 根目录
$pluginInstallDir = "$classIslandRoot\data\Plugins\entertainingisland.app"      # 插件安装目录

# 自动检测 ClassIsland 可执行文件位置（兼容新旧版本）
$classIslandExe = "$classIslandRoot\ClassIsland.exe"
if (-not (Test-Path $classIslandExe)) {
    # 旧版: ClassIsland_App\ClassIsland.Desktop.exe
    $legacyExe = "$classIslandRoot\ClassIsland_App\ClassIsland.Desktop.exe"
    if (Test-Path $legacyExe) {
        $classIslandExe = $legacyExe
    } else {
        # 新版: app-{version}\ClassIsland.Desktop.exe
        $appFolder = Get-ChildItem -Path $classIslandRoot -Directory -Filter "app-*" -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($appFolder) {
            $classIslandExe = Join-Path $appFolder.FullName "ClassIsland.Desktop.exe"
        }
    }
}

Write-Host "=== ClassIsland 插件部署脚本 ===" -ForegroundColor Cyan
Write-Host "  项目目录:   $pluginProject"
Write-Host "  ClassIsland: $classIslandRoot"
Write-Host "  可执行文件: $classIslandExe"
Write-Host ""

# 0. 关闭已运行的 ClassIsland
Write-Host "🛑 正在关闭已有的 ClassIsland 进程..." -ForegroundColor Yellow
$processes = Get-Process -Name "ClassIsland*" -ErrorAction SilentlyContinue
if ($processes) {
    $processes | ForEach-Object {
        Write-Host "   终止: $($_.ProcessName) (PID: $($_.Id))"
        $_.Kill()
    }
    Start-Sleep -Seconds 2
    Write-Host "✅ 已关闭旧进程" -ForegroundColor Green
} else {
    Write-Host "   没有运行中的 ClassIsland 进程"
}

# 1. 构建插件
Write-Host "`n🔨 正在构建插件..." -ForegroundColor Yellow
Set-Location $pluginProject
dotnet build -c Debug
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ 构建失败！" -ForegroundColor Red
    exit 1
}
Write-Host "✅ 构建成功" -ForegroundColor Green

# 2. 检查 ClassIsland 是否存在
if (-not (Test-Path $classIslandExe)) {
    Write-Host "`n⚠️  ClassIsland 未找到: $classIslandExe" -ForegroundColor Red
    Write-Host "   请运行 setup.bat 自动下载，或手动下载解压到: $classIslandRoot" -ForegroundColor Red
    Write-Host "   下载地址: https://github.com/ClassIsland/ClassIsland/releases/latest" -ForegroundColor Red
    exit 1
}

# 3. 复制插件到 ClassIsland Plugins 目录
Write-Host "`n📦 正在部署插件..." -ForegroundColor Yellow
if (Test-Path $pluginInstallDir) {
    Remove-Item $pluginInstallDir -Recurse -Force
}
New-Item -ItemType Directory -Path $pluginInstallDir -Force | Out-Null

# 复制所有构建输出
Copy-Item "$pluginOutput\*" $pluginInstallDir -Recurse -Force

Write-Host "✅ 插件已部署到: $pluginInstallDir" -ForegroundColor Green
Get-ChildItem $pluginInstallDir -File | Select-Object Name, @{N='Size';E={$_.Length}} | Format-Table -AutoSize

# 4. 启动 ClassIsland
if ($Debug) {
    Write-Host "`n🚀 正在启动 ClassIsland（调试模式）..." -ForegroundColor Yellow
    Write-Host "   请在 IDE 中附加调试器或使用热重载。" -ForegroundColor DarkGray
}

Write-Host "`n🚀 启动 ClassIsland..." -ForegroundColor Green
Start-Process $classIslandExe

Write-Host "`n=== 部署完成 ===" -ForegroundColor Cyan
Write-Host "插件 ID: entertainingisland.app"
Write-Host "安装路径: $pluginInstallDir"
