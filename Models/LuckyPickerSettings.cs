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
