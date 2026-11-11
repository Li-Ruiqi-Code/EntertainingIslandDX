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

using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Shared;
using EntertainingIsland.Models;

namespace EntertainingIsland.Views.Components;

public partial class CatchphraseComponentSettingsControl : ComponentBase<CatchphraseComponentSettings>
{
    public ObservableCollection<CatchphrasePreset> GlobalPresets =>
        IAppHost.GetService<Plugin>().Settings.CatchphrasePresets;

    /// <summary>主插件设置（用于绑定全局开关）</summary>
    public Settings PluginSettings => IAppHost.GetService<Plugin>().Settings;

    public CatchphraseComponentSettingsControl()
    {
        InitializeComponent();
        AddBtn.Click += (_, _) =>
        {
            GlobalPresets.Add(new CatchphrasePreset { Phrase = "新口头禅" });
        };
        ClearBtn.Click += (_, _) =>
        {
            IAppHost.TryGetService<CatchphraseStore>()?.Clear();
        };
    }

    private void DeletePreset_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is CatchphrasePreset preset)
            GlobalPresets.Remove(preset);
    }
}
