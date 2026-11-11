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

using Avalonia.Threading;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Attributes;
using ClassIsland.Shared;
using EntertainingIsland.Models;
using EntertainingIsland.Services;

namespace EntertainingIsland.Views.Components;

[ComponentInfo(
    "E5D1D45A-5DA0-4BA9-9075-2466ADF636E0",
    "口头禅记录",
    "\uE3E4",
    "记录老师口头禅，支持语音识别自动检测"
)]
public partial class CatchphraseComponent : ComponentBase<CatchphraseComponentSettings>
{
    private CatchphraseStore? _store;
    private CatchphraseSpeechService? _speechService;
    private Settings? _pluginSettings;

    public CatchphraseComponent()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        // 全局显隐
        var state = IAppHost.TryGetService<EntertainmentState>();
        if (state != null)
            state.PropertyChanged += (s, a) =>
            {
                var name = a.PropertyName;
                if (name == nameof(EntertainmentState.IsHidden))
                    IsVisible = !state.IsHidden;
                else if (name == nameof(EntertainmentState.CatchphraseClearRequest))
                    _store?.Clear();
            };

        // 通过 DI 获取单例 CatchphraseStore（与自动化行动共享同一实例）
        _store = IAppHost.TryGetService<CatchphraseStore>();
        if (_store != null)
            _store.PropertyChanged += (_, _) => RefreshDisplay();

        // 获取插件设置
        _pluginSettings = IAppHost.GetService<Plugin>().Settings;

        // 初始化语音识别
        InitSpeechRecognition();

        RefreshDisplay();
    }

    protected override void OnDetachedFromVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        StopSpeechRecognition();
        base.OnDetachedFromVisualTree(e);
    }

    public void RefreshDisplay()
    {
        if (_store != null)
            CatchphraseLabel.Text = _store.GetDisplayText();
    }

    private void InitSpeechRecognition()
    {
        if (_pluginSettings == null || _store == null) return;

        // 监听语音识别开关变化
        _pluginSettings.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(Models.Settings.CatchphraseVoiceEnabled))
            {
                if (_pluginSettings.CatchphraseVoiceEnabled)
                    StartSpeechRecognition();
                else
                    StopSpeechRecognition();
            }
        };

        // 根据初始设置启动
        if (_pluginSettings.CatchphraseVoiceEnabled)
            StartSpeechRecognition();
    }

    private void StartSpeechRecognition()
    {
        if (_speechService != null || _pluginSettings == null || _store == null) return;

        _speechService = new CatchphraseSpeechService(_store, _pluginSettings);
        _speechService.PhraseMatched += phrase =>
        {
            // 计数已在 CatchphraseSpeechService 内完成，这里只刷新界面并处理强调提醒
            Dispatcher.UIThread.Post(() =>
            {
                RefreshDisplay();
                if (_pluginSettings?.CatchphraseEmphasisEnabled == true)
                    IAppHost.TryGetService<CatchphraseNotifier>()?.ShowEmphasis(phrase);
            });
        };
        _speechService.Start();
    }

    private void StopSpeechRecognition()
    {
        if (_speechService == null) return;
        _speechService.Stop();
        _speechService.Dispose();
        _speechService = null;
    }
}
