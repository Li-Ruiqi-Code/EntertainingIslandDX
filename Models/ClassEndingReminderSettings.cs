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
/// 下课倒计时提醒设置（对齐 ClassIsland 内置"即将上课"提醒设置结构）
/// </summary>
public partial class ClassEndingReminderSettings : ObservableObject
{
    /// <summary>是否启用下课倒计时提醒</summary>
    [ObservableProperty]
    private bool _isEnabled = true;

    /// <summary>下课前多少秒触发提醒（默认 60 秒 = 1 分钟）</summary>
    [ObservableProperty]
    private int _deltaTime = 60;

    /// <summary>提醒遮罩文字</summary>
    [ObservableProperty]
    private string _maskText = "即将下课";

    /// <summary>提醒详细文字（叠加层第二页轮播显示）</summary>
    [ObservableProperty]
    private string _overlayText = "本堂课即将结束，请注意整理笔记。";

    /// <summary>是否显示教师名称</summary>
    [ObservableProperty]
    private bool _showTeacherName;

    /// <summary>是否启用强调效果（使用 ClassIsland 主题色涟漪动画）</summary>
    [ObservableProperty]
    private bool _enableEmphasisEffect = true;

    /// <summary>是否启用语音播报</summary>
    [ObservableProperty]
    private bool _enableSpeech;

    /// <summary>是否启用提示音效</summary>
    [ObservableProperty]
    private bool _enableSound;
}
