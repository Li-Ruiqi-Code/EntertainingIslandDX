using CommunityToolkit.Mvvm.ComponentModel;

namespace EntertainingIsland.Models;

/// <summary>
/// 口头禅预设条目：一个短语。计数由语音识别或自动化行动触发。
/// </summary>
public partial class CatchphrasePreset : ObservableObject
{
    [ObservableProperty]
    private string _phrase = "";
}
