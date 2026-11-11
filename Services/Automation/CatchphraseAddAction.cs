using System;
using System.Threading.Tasks;
using ClassIsland.Core.Abstractions.Automation;
using ClassIsland.Core.Attributes;
using ClassIsland.Shared;
using EntertainingIsland.Models;
using EntertainingIsland.Views.Automation;

namespace EntertainingIsland.Services.Automation;

/// <summary>
/// 把指定的一句口头禅计数 +1。全局热键移除后，这是手动记一次口头禅的主要入口。
/// 可在 ClassIsland 自动化规则中配置为按下某个自定义快捷键或按时间触发。
/// </summary>
[ActionInfo("entertainingisland.catchphrase.add", "口头禅 +1", "\uE710", defaultGroupToMenu: "EntertainingIsland/口头禅")]
public class CatchphraseAddAction : ActionBase<CatchphraseAddActionSettings>
{
    protected override Task OnInvoke()
    {
        var phrase = Settings.Phrase;

        // 未指定文字时，退回到第一个预设口头禅
        if (string.IsNullOrWhiteSpace(phrase))
        {
            var presets = IAppHost.TryGetService<Plugin>()?.Settings.CatchphrasePresets;
            if (presets is { Count: > 0 })
                phrase = presets[0].Phrase;
        }

        if (string.IsNullOrWhiteSpace(phrase))
            return Task.CompletedTask;

        IAppHost.TryGetService<CatchphraseStore>()?.Add(phrase);
        Logger.Info($"口头禅 +1（自动化）: \"{phrase}\"");

        // 强调提醒
        if (IAppHost.TryGetService<Plugin>()?.Settings.CatchphraseEmphasisEnabled == true)
            IAppHost.TryGetService<CatchphraseNotifier>()?.ShowEmphasis(phrase);

        return Task.CompletedTask;
    }
}
