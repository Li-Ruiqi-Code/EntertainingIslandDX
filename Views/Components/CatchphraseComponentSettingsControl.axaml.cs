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
