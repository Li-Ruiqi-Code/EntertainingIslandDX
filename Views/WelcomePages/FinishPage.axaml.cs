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

using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using ClassIsland.Shared;
using EntertainingIsland.ViewModels;

namespace EntertainingIsland.Views.WelcomePages;

public partial class FinishPage : UserControl, IWelcomePage
{
    public WelcomeViewModel ViewModel { get; set; } = null!;

    public FinishPage()
    {
        InitializeComponent();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        Root.Classes.Add("anim");
        try
        {
            var plugin = IAppHost.TryGetService<Plugin>();
            if (plugin != null)
            {
                var p = Path.Combine(plugin.Info.PluginFolderPath, "banner.png");
                if (File.Exists(p)) BannerImage.Source = new Bitmap(p);
            }
        }
        catch { }
    }

    private void ButtonFinish_OnClick(object? sender, RoutedEventArgs e)
    {
        WelcomeWindow.FromControl(this)?.FinishWizard();
    }
}
