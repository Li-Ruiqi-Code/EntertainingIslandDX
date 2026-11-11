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
