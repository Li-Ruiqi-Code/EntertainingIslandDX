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

using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using EntertainingIsland.ViewModels;
using FluentAvalonia.UI.Controls;

namespace EntertainingIsland.Views.WelcomePages;

public partial class QuickSetupPage : UserControl, IWelcomePage
{
    private WelcomeViewModel _vm = null!;
    public WelcomeViewModel ViewModel
    {
        get => _vm;
        set { _vm = value; DataContext = value; }
    }

    public QuickSetupPage()
    {
        InitializeComponent();
        DataContext = this;
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        // 确保 ViewModel 绑定已就绪
        if (_vm == null) return;

        // 卡片入场顺序
        var cards = new SettingsExpander[]
        {
            CardNovelReader, CardFortune, CardCatchphrase, CardSports, CardCameraStatus,
        };

        // 逐个显示卡片（入场动画）
        foreach (var card in cards)
        {
            await Task.Delay(80);
            card.Classes.Add("visible");
        }
    }

    private void ButtonNext_OnClick(object? sender, RoutedEventArgs e) =>
        WelcomeWindow.FromControl(this)?.NavigateForward();

    private void ButtonBack_OnClick(object? sender, RoutedEventArgs e) =>
        WelcomeWindow.FromControl(this)?.NavigateBack();
}
