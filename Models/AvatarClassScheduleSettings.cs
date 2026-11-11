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

using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EntertainingIsland.Models;

/// <summary>
/// 头像课程表组件设置
/// </summary>
public partial class AvatarClassScheduleSettings : ObservableObject
{
    /// <summary>科目到头像图片路径的映射（科目名 → 文件路径）</summary>
    [ObservableProperty]
    private Dictionary<string, string> _subjectAvatarMap = new();

    /// <summary>头像圆角半径（0-50）</summary>
    [ObservableProperty]
    private double _avatarCornerRadius = 50.0;

    /// <summary>已上课科目头像不透明度（0-1）</summary>
    [ObservableProperty]
    private double _pastClassOpacity = 0.4;

    /// <summary>已上课科目头像饱和度（0-1）</summary>
    [ObservableProperty]
    private double _pastClassSaturation = 0.4;

    /// <summary>未上课科目头像不透明度（0-1）</summary>
    [ObservableProperty]
    private double _upcomingClassOpacity = 0.85;

    /// <summary>是否在头像旁显示科目名称</summary>
    [ObservableProperty]
    private bool _showSubjectName = true;

    /// <summary>是否显示上课时间</summary>
    [ObservableProperty]
    private bool _showClassTime = true;

    /// <summary>是否显示倒计时/进度</summary>
    [ObservableProperty]
    private bool _showCountdown = true;

    /// <summary>头像默认大小（像素）</summary>
    [ObservableProperty]
    private double _avatarSize = 32.0;

    /// <summary>已上课头像大小（像素）</summary>
    [ObservableProperty]
    private double _pastAvatarSize = 32.0;

    /// <summary>当前课头像大小（像素，高亮放大）</summary>
    [ObservableProperty]
    private double _currentAvatarSize = 40.0;

    /// <summary>未上课头像大小（像素）</summary>
    [ObservableProperty]
    private double _upcomingAvatarSize = 32.0;

    /// <summary>已上课是否隐藏</summary>
    [ObservableProperty]
    private bool _hidePastClasses;

    /// <summary>上课时仅显示当前课程</summary>
    [ObservableProperty]
    private bool _showOnlyCurrentInClass;

    /// <summary>头像间距（像素，0-16）</summary>
    [ObservableProperty]
    private double _avatarSpacing = 4.0;

    /// <summary>无课程时显示占位符</summary>
    [ObservableProperty]
    private bool _showPlaceholder = true;

    /// <summary>启用头像入场动画</summary>
    [ObservableProperty]
    private bool _enableEntranceAnimation = true;

    /// <summary>占位符文字</summary>
    [ObservableProperty]
    private string _placeholderText = "今天没有课";

    /// <summary>已上课科目头像不透明度（0-1），默认 0.4</summary>
    // [ObservableProperty] already defined above with _pastClassOpacity = 0.4
    // Override default: keep PastClassOpacity = 0.4

    /// <summary>已上课科目头像饱和度（0-1），默认 0.4</summary>
    // [ObservableProperty] already defined above with _pastClassSaturation = 0.4 (was 0.2)
}
