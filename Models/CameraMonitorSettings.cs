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

using CommunityToolkit.Mvvm.ComponentModel;

namespace EntertainingIsland.Models;

/// <summary>
/// 摄像头监控设置 — 仅摄像头，不包含麦克风。
/// </summary>
public partial class CameraMonitorSettings : ObservableObject
{
    private bool _enableCameraMonitor = true;
    private int _pollingIntervalMs = 2000;

    /// <summary>是否启用摄像头监控</summary>
    public bool EnableCameraMonitor
    {
        get => _enableCameraMonitor;
        set => SetProperty(ref _enableCameraMonitor, value);
    }

    /// <summary>轮询间隔（毫秒），最小 500</summary>
    public int PollingIntervalMs
    {
        get => _pollingIntervalMs;
        set => SetProperty(ref _pollingIntervalMs, Math.Max(value, 500));
    }
}
