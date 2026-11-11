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
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using ClassIsland.Core;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Enums.SettingsWindow;
using ClassIsland.Shared;
using ClassIsland.Shared.Helpers;
using EntertainingIsland.Models;

namespace EntertainingIsland.Views.SettingsPages;

[SettingsPageInfo(
    "entertainingisland.settings",
    "EntertainingIsland",
    "\uEF27",
    "\uEF27",
    SettingsPageCategory.External
)]
public partial class MySettingsPage : SettingsPageBase
{
    private Plugin PluginEntry => IAppHost.GetService<Plugin>();
    public Settings Settings => PluginEntry.Settings;
    public FeatureToggles Ft => Settings.FeatureToggles;

    public string PluginInfoText =>
        $"EntertainingIsland\n" +
        $"版本: {PluginEntry.Info.Manifest.Version}\n" +
        $"2026 xxtsoft 对一切提前开学&违规补课等行为致以最强烈的谴责";

    public MySettingsPage()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void ButtonReRunOobe_OnClick(object? sender, RoutedEventArgs e)
    {
        Plugin.ShowWelcomeWizard();
    }

    private async void ButtonClearData_OnClick(object? sender, RoutedEventArgs e)
    {
        var owner = TopLevel.GetTopLevel(this) as Window;

        // 确认对话框
        if (!await ShowConfirmDialog(owner)) return;

        // 执行清除
        try
        {
            var pluginDir = PluginEntry.PluginConfigFolder;

            // 删除所有文件
            foreach (var file in Directory.GetFiles(pluginDir, "*.*", SearchOption.TopDirectoryOnly))
            {
                try { File.Delete(file); } catch { }
            }

            // 重置内存中的设置
            PluginEntry.Settings = new Settings { HasSeenWelcome = false };

            // 保存重置后的配置
            ConfigureFileHelper.SaveConfig(Path.Combine(pluginDir, "Settings.json"), PluginEntry.Settings);

            // 显示成功提示
            await ShowInfoDialog(owner, "数据已清除", "所有数据已清除。将自动弹出欢迎向导引导你重新设置。");

            // 触发 OOBE
            Plugin.ShowWelcomeWizard();
        }
        catch (System.Exception ex)
        {
            await ShowInfoDialog(owner, "清除失败", $"清除数据时发生错误：{ex.Message}");
        }
    }

    private static Task<bool> ShowConfirmDialog(Window? owner)
    {
        var tcs = new TaskCompletionSource<bool>();

        var dialog = new Window
        {
            Title = "确认清除数据",
            Width = 440,
            Height = 220,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ShowInTaskbar = false,
            CanResize = false,
        };

        var panel = new StackPanel { Margin = new Thickness(24, 20) };
        panel.Children.Add(new TextBlock
        {
            Text = "确认清除所有数据？",
            FontSize = 18,
            FontWeight = FontWeight.SemiBold,
        });
        panel.Children.Add(new TextBlock
        {
            Text = "这将删除所有设置、口头禅记录和缓存文件，恢复为全新安装状态。\n此操作不可撤销。",
            FontSize = 13,
            Opacity = 0.7,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 8, 0, 0),
        });

        var btnPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 8,
            Margin = new Thickness(0, 20, 0, 0),
        };
        var cancelBtn = new Button { Content = "取消" };
        var confirmBtn = new Button
        {
            Content = "确认清除",
            Foreground = Brushes.White,
            Background = Brushes.Red,
        };
        cancelBtn.Click += (_, _) => { tcs.TrySetResult(false); dialog.Close(); };
        confirmBtn.Click += (_, _) => { tcs.TrySetResult(true); dialog.Close(); };
        btnPanel.Children.Add(cancelBtn);
        btnPanel.Children.Add(confirmBtn);
        panel.Children.Add(btnPanel);
        dialog.Content = panel;

        if (owner != null)
            dialog.ShowDialog(owner);
        else
            dialog.Show();

        return tcs.Task;
    }

    private static async Task ShowInfoDialog(Window? owner, string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 360,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            ShowInTaskbar = false,
            CanResize = false,
        };
        var panel = new StackPanel { Margin = new Thickness(24, 20) };
        panel.Children.Add(new TextBlock
        {
            Text = title,
            FontSize = 18,
            FontWeight = FontWeight.SemiBold,
        });
        panel.Children.Add(new TextBlock
        {
            Text = message,
            FontSize = 13,
            Opacity = 0.7,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 8, 0, 0),
        });
        var okBtn = new Button
        {
            Content = "确定",
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness(0, 16, 0, 0),
        };
        var tcs = new TaskCompletionSource<bool>();
        okBtn.Click += (_, _) => { tcs.TrySetResult(true); dialog.Close(); };
        panel.Children.Add(okBtn);
        dialog.Content = panel;

        if (owner != null)
            await dialog.ShowDialog<bool>(owner);
        else
            dialog.Show();
    }
}