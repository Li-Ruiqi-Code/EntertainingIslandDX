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
using System.Collections.ObjectModel;

namespace EntertainingIsland.Models;

/// <summary>
/// 点名器设置
/// </summary>
public partial class LuckyPickerSettings : ObservableObject
{
    [ObservableProperty]
    private bool _isEnabled = true;

    /// <summary>名单文本（每行一个姓名）</summary>
    [ObservableProperty]
    private string _nameListText = "";

    /// <summary>通知持续秒数</summary>
    [ObservableProperty]
    private int _notificationDurationSeconds = 5;

    /// <summary>按钮文字</summary>
    [ObservableProperty]
    private string _buttonText = "点名";

    /// <summary>是否显示最近被点中的人</summary>
    [ObservableProperty]
    private bool _showLastPicked = true;

    /// <summary>是否显示持久化二级提醒。开启后先弹出强调提示，再显示持续 N 秒的完整提醒。</summary>
    [ObservableProperty]
    private bool _showPersistentOverlay = true;

    /// <summary>最近被点中的人（内部使用，不持久化到设置）</summary>
    [ObservableProperty]
    private string _lastPickedName = "";
}
