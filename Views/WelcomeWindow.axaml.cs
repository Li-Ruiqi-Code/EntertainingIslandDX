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
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ClassIsland.Core.Controls;
using ClassIsland.Shared;
using EntertainingIsland.ViewModels;
using EntertainingIsland.Views.WelcomePages;
using FluentAvalonia.UI.Controls;

namespace EntertainingIsland.Views;

public partial class WelcomeWindow : MyWindow, INavigationPageFactory
{
    public WelcomeViewModel ViewModel { get; } = new();

    private readonly List<Type> _pages =
    [
        typeof(SoundChoicePage),
        typeof(IntroPage),
        typeof(FeaturesPage),
        typeof(QuickSetupPage),
        typeof(FinishPage),
    ];

    private readonly Dictionary<Type, object?> _pageCache = new();

    public WelcomeWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    public Control? GetPage(Type srcType)
    {
        if (_pageCache.TryGetValue(srcType, out var v) && v is Control control)
            return control;

        var page = Activator.CreateInstance(srcType);
        if (page is IWelcomePage welcomePage)
            welcomePage.ViewModel = ViewModel;

        ViewModel.CurrentPage = srcType;
        _pageCache[srcType] = page;
        return page as Control;
    }

    public Control? GetPageFromObject(object target) => null;

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        MainFrame.Navigate(_pages[0]);
        ViewModel.CurrentPage = _pages[0];
    }

    public void NavigateForward()
    {
        var current = ViewModel.CurrentPage ?? _pages[0];
        var index = Math.Min(_pages.IndexOf(current) + 1, _pages.Count - 1);
        MainFrame.Navigate(_pages[index]);
        ViewModel.CurrentPage = _pages[index];
    }

    public void NavigateBack()
    {
        var current = ViewModel.CurrentPage ?? _pages[0];
        var index = Math.Max(_pages.IndexOf(current) - 1, 0);
        MainFrame.Navigate(_pages[index]);
        ViewModel.CurrentPage = _pages[index];
    }

    public void FinishWizard()
    {
        var plugin = IAppHost.TryGetService<Plugin>();
        if (plugin != null)
        {
            var s = plugin.Settings;
            var ft = s.FeatureToggles;

            ft.NovelReader = ViewModel.EnableNovelReader;
            ft.Fortune = ViewModel.EnableFortune;
            ft.Catchphrase = ViewModel.EnableCatchphrase;
            ft.Sports = ViewModel.EnableSports;
            ft.CameraStatus = ViewModel.EnableCameraStatus;

            s.HasSeenWelcome = true;
            var configPath = Path.Combine(plugin.PluginConfigFolder, "Settings.json");
            ClassIsland.Shared.Helpers.ConfigureFileHelper.SaveConfig(configPath, s);
        }

        ViewModel.IsWizardCompleted = true;
        Close();
    }

    public static WelcomeWindow? FromControl(Control c) =>
        TopLevel.GetTopLevel(c) as WelcomeWindow;
}
