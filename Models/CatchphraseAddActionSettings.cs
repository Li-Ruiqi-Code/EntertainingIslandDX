using CommunityToolkit.Mvvm.ComponentModel;

namespace EntertainingIsland.Models;

/// <summary>
/// 「口头禅 +1」自动化行动的设置。
/// </summary>
public partial class CatchphraseAddActionSettings : ObservableObject
{
    /// <summary>要 +1 的口头禅文字。</summary>
    [ObservableProperty]
    private string _phrase = "";
}
