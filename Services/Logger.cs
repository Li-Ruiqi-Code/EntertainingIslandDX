// EntertainingIslandDX - ClassIsland 娱乐功能插件
// Copyright (C) 2026 Li-Ruiqi-Code
// Copyright (C) 2025 xxt8582753 (BSOD-MEMZ) - 原始 EntertainingIsland 项目
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.IO;

namespace EntertainingIsland.Services;

/// <summary>
/// 简单的文件日志器，将日志写入插件配置目录下的 Logs 文件夹。
/// 方便不会 C# 的用户查看运行状态。
/// </summary>
public static class Logger
{
    private static string LogDir = null!;
    private static readonly object LockObj = new();

    /// <summary>
    /// 初始化日志目录。应在插件 Initialize 时调用。
    /// </summary>
    public static void Initialize(string pluginConfigFolder)
    {
        LogDir = Path.Combine(pluginConfigFolder, "Logs");
        try { Directory.CreateDirectory(LogDir); } catch { }
    }

    public static void Info(string message) => Write("INFO", message);

    public static void Error(string message) => Write("ERROR", message);

    public static void Warn(string message) => Write("WARN", message);

    private static void Write(string level, string message)
    {
        if (LogDir == null) return;

        try
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var logFile = Path.Combine(LogDir, $"teacher-alert-{today}.log");
            var line = $"[{DateTime.Now:HH:mm:ss.fff}] [{level}] {message}";
            lock (LockObj)
            {
                File.AppendAllText(logFile, line + Environment.NewLine);
            }
        }
        catch
        {
            // 日志写入失败不应影响主功能
        }
    }

    /// <summary>
    /// 获取最新日志文件路径
    /// </summary>
    public static string GetLatestLogPath()
    {
        try
        {
            if (LogDir == null || !Directory.Exists(LogDir))
                return "(未初始化)";

            var files = Directory.GetFiles(LogDir, "teacher-alert-*.log");
            return files.Length > 0
                ? files.OrderByDescending(f => f).First()
                : Path.Combine(LogDir, "(暂无日志)");
        }
        catch
        {
            return "(获取失败)";
        }
    }
}
