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
/// 功能开关。用户在 EntertainingIsland 设置中可以自由启用/禁用各组件、提醒提供方和自动化注册项。
/// 禁用后，对应的功能不会在 ClassIsland 的任何界面中出现。
/// </summary>
public partial class FeatureToggles : ObservableObject
{
    // ===== 完整功能模块（控制整个模块的注册，包括设置页、服务、浮窗等） =====
    [ObservableProperty]
    private bool _luckyPicker = true;

    // ===== 组件 =====
    [ObservableProperty]
    private bool _novelReader = true;

    [ObservableProperty]
    private bool _catchphrase = true;

    [ObservableProperty]
    private bool _avatarClassSchedule = true;

    [ObservableProperty]
    private bool _rssNews = true;

    [ObservableProperty]
    private bool _sports = true;

    [ObservableProperty]
    private bool _cameraStatus = true;

    [ObservableProperty]
    private bool _fortune = true;

    // ===== 提醒提供方 =====
    [ObservableProperty]
    private bool _classEndingReminder = true;

    [ObservableProperty]
    private bool _luckyPickerNotifier = true;

    // ===== 自动化行动 =====
    [ObservableProperty]
    private bool _toggleVisibility = true;

    [ObservableProperty]
    private bool _showAllComponents = true;

    [ObservableProperty]
    private bool _hideAllComponents = true;

    [ObservableProperty]
    private bool _novelActions = true;

    [ObservableProperty]
    private bool _rssActions = true;

    [ObservableProperty]
    private bool _catchphraseClearAction = true;

    [ObservableProperty]
    private bool _catchphraseAddAction = true;

    [ObservableProperty]
    private bool _cameraActions = true;

    // ===== 自动化触发器 =====
    [ObservableProperty]
    private bool _cameraTriggers = true;
}
