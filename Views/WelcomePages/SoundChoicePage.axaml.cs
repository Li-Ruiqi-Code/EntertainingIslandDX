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
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassIsland.Shared;
using EntertainingIsland.ViewModels;

namespace EntertainingIsland.Views.WelcomePages;

public partial class SoundChoicePage : UserControl, IWelcomePage
{
    public WelcomeViewModel ViewModel { get; set; } = null!;

    public SoundChoicePage()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        // 页面一加载就播放 12s 无声 WAV，保持音频设备唤醒
        try
        {
            var plugin = IAppHost.TryGetService<Plugin>();
            if (plugin != null)
            {
                var path = Path.Combine(plugin.Info.PluginFolderPath, "musei.wav");
                if (File.Exists(path))
                    IntroPage.PlaySoundStatic(path);
            }
        }
        catch { }
    }

    private void ButtonMute_OnClick(object? sender, RoutedEventArgs e)
    {
        // 停止无声预热
        IntroPage.StopSoundStatic();
        Plugin.OobeSoundEnabled = false;
        WelcomeWindow.FromControl(this)?.NavigateForward();
    }

    private void ButtonSound_OnClick(object? sender, RoutedEventArgs e)
    {
        Plugin.OobeSoundEnabled = true;

        // 停止无声预热，切入正式 intro（设备已持续唤醒，零延迟）
        IntroPage.StopSoundStatic();

        try
        {
            var plugin = IAppHost.TryGetService<Plugin>();
            if (plugin != null)
            {
                var path = Path.Combine(plugin.Info.PluginFolderPath, "intro.wav");
                if (File.Exists(path))
                    IntroPage.PlaySoundStatic(path);
            }
        }
        catch { }

        WelcomeWindow.FromControl(this)?.NavigateForward();
    }
}
