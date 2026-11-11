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
using System.Linq;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ClassIsland.Core;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Enums.SettingsWindow;
using ClassIsland.Shared;
using EntertainingIsland.Models;

namespace EntertainingIsland.Views.SettingsPages;

[SettingsPageInfo(
    "entertainingisland.luckypicker",
    "点名器",
    "\uE716",
    "\uE716",
    SettingsPageCategory.External
)]
public partial class LuckyPickerSettingsPage : SettingsPageBase
{
    private Plugin PluginEntry => IAppHost.GetService<Plugin>();
    public Settings Settings => PluginEntry.Settings;

    public LuckyPickerSettingsPage()
    {
        InitializeComponent();
        DataContext = this;

        // 名单导入按钮
        ImportBtn.Click += async (_, _) =>
        {
            var window = TopLevel.GetTopLevel(this) ?? AppBase.Current.GetRootWindow();
            var files = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "选择名单文件（.txt）",
                AllowMultiple = false
            });
            if (files.Count > 0)
            {
                try
                {
                    var text = await File.ReadAllTextAsync(files[0].Path.LocalPath);
                    Settings.LuckyPicker.NameListText = text;
                }
                catch (Exception ex)
                {
                    NameCountLabel.Text = $"❌ 导入失败: {ex.Message}";
                }
            }
        };

        // 监听名单变化，更新计数
        Settings.LuckyPicker.PropertyChanged += (_, a) =>
        {
            if (a.PropertyName == nameof(LuckyPickerSettings.NameListText))
                UpdateNameCount();
        };

        UpdateNameCount();
    }

    private void UpdateNameCount()
    {
        var text = Settings.LuckyPicker.NameListText ?? "";
        var names = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(n => n.Trim())
                        .Where(n => n.Length > 0)
                        .ToList();
        NameCountLabel.Text = $"当前名单共 {names.Count} 人";
    }
}
