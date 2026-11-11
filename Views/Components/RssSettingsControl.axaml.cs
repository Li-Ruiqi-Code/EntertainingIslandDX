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
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassIsland.Core.Abstractions.Controls;
using EntertainingIsland.Models;

namespace EntertainingIsland.Views.Components;

public partial class RssSettingsControl : ComponentBase<RssComponentSettings>
{
    public ObservableCollection<PresetItem> PresetSources { get; } = new(
        RssPresets.Feeds.Select(f => new PresetItem { Name = f.Name, Url = f.Url }));

    public RssSettingsControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void OnPresetClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string url)
            Settings.RssFeedUrl = url;
    }

    public class PresetItem
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
    }
}
