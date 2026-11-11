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

using Avalonia.Controls;
using ClassIsland.Core.Abstractions.Controls;
using EntertainingIsland.Models;

namespace EntertainingIsland.Views.Components;

public partial class SportsSettingsControl : ComponentBase<SportsSettings>
{
    public SportsSettingsControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    protected override void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        // 填充联赛下拉框
        foreach (var p in SportsSettings.LeaguePresets)
            LeagueCombo.Items.Add(p.Name);

        // 同步选中项：默认选"全部"（index 0）
        var preset = SportsSettings.LeaguePresets.FirstOrDefault(p => p.Id == Settings.SelectedLeagueId);
        var idx = preset != null ? SportsSettings.LeaguePresets.IndexOf(preset) : 0;
        LeagueCombo.SelectedIndex = idx;

        LeagueCombo.SelectionChanged += (_, _) =>
        {
            if (LeagueCombo.SelectedIndex >= 0 && LeagueCombo.SelectedIndex < SportsSettings.LeaguePresets.Count)
                Settings.SelectedLeagueId = SportsSettings.LeaguePresets[LeagueCombo.SelectedIndex].Id;
        };
    }
}
