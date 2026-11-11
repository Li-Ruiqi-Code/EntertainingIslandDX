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
