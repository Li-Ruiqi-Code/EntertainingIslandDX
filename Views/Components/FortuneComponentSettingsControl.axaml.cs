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
using Avalonia.Interactivity;
using ClassIsland.Core;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Shared;
using EntertainingIsland.Models;
using EntertainingIsland.Services;

namespace EntertainingIsland.Views.Components;

public partial class FortuneComponentSettingsControl : ComponentBase<FortuneComponentSettings>
{
    private Plugin PluginEntry => IAppHost.GetService<Plugin>();
    public Settings PluginSettings => PluginEntry.Settings;

    public FortuneComponentSettingsControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void AddGood_Click(object? sender, RoutedEventArgs e)
    {
        PluginSettings.Fortune.GoodFortunes.Add(new FortuneEntry { MainText = "新条目", SubText = "副标题" });
    }

    private void DeleteGood_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is FortuneEntry entry)
            PluginSettings.Fortune.GoodFortunes.Remove(entry);
    }

    private void AddBad_Click(object? sender, RoutedEventArgs e)
    {
        PluginSettings.Fortune.BadFortunes.Add(new FortuneEntry { MainText = "新条目", SubText = "副标题" });
    }

    private void DeleteBad_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is FortuneEntry entry)
            PluginSettings.Fortune.BadFortunes.Remove(entry);
    }

    private void Refresh_Click(object? sender, RoutedEventArgs e)
    {
        IAppHost.TryGetService<FortuneService>()?.RefreshToday(forceRandom: true);
    }
}
