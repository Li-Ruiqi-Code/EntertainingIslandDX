using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace EntertainingIsland.Models;

public partial class Settings : ObservableObject
{
    [ObservableProperty]
    private bool _showWelcomeMessage = true;

    /// <summary>是否已完成首次欢迎向导</summary>
    [ObservableProperty]
    private bool _hasSeenWelcome = false;

    // ===== 口头禅记录 =====

    [ObservableProperty]
    private ObservableCollection<CatchphrasePreset> _catchphrasePresets = new()
    {
        new() { Phrase = "不要讲话" },
        new() { Phrase = "安静" },
        new() { Phrase = "看黑板" },
    };

    [ObservableProperty]
    private bool _catchphraseClearOnNewDay = true;

    /// <summary>老师每说一次口头禅，弹出 1 秒强调提醒</summary>
    [ObservableProperty]
    private bool _catchphraseEmphasisEnabled;

    /// <summary>启用语音识别自动检测口头禅</summary>
    [ObservableProperty]
    private bool _catchphraseVoiceEnabled;

    /// <summary>语音识别置信度阈值 (0.0~1.0)，低于此值的识别结果将被忽略</summary>
    [ObservableProperty]
    private double _catchphraseVoiceConfidence = 0.5;

    // ===== 摄像头安全检测 =====

    [ObservableProperty]
    private CameraMonitorSettings _cameraMonitor = new();

    // ===== 每日运势 =====

    [ObservableProperty]
    private FortuneSettings _fortune = new();

    // ===== 公平点名器 =====

    [ObservableProperty]
    private LuckyPickerSettings _luckyPicker = new();

    // ===== 功能开关 =====

    [ObservableProperty]
    private FeatureToggles _featureToggles = new();
}
