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
using Avalonia.Media;
using Avalonia.Threading;
using ClassIsland.Core;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Attributes;
using ClassIsland.Shared;
using EntertainingIsland.Models;
using EntertainingIsland.Services;

namespace EntertainingIsland.Views.Components;

[ComponentInfo(
    "3D6DEF83-0E05-44DD-944B-0A44D439BD56",
    "小说阅读器",
    "\uE215",
    "读取 txt 小说并在主界面显示，支持快捷键翻页和自动保存进度"
)]
public partial class NovelReaderComponent : ComponentBase<NovelReaderSettings>
{
    private System.Timers.Timer? _flipTimer;
    private string _novelText = "";
    private string _flatText = "";
    private int _currentCharIndex;

    // 暂停状态
    private bool _isPaused;

    public NovelReaderComponent()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        var state = IAppHost.TryGetService<EntertainmentState>();
        if (state != null)
        {
            state.PropertyChanged += (s, a) =>
            {
                var name = a.PropertyName;
                if (name == nameof(EntertainmentState.IsHidden))
                    IsVisible = !state.IsHidden;
                else if (name == nameof(EntertainmentState.NovelRestartRequest))
                {
                    _currentCharIndex = 0;
                    SaveProgress();
                    UpdateDisplay();
                }
                else if (name == nameof(EntertainmentState.NovelPauseRequest) && !_isPaused)
                {
                    _isPaused = true;
                    _flipTimer?.Stop();
                    UpdateDisplay();
                }
                else if (name == nameof(EntertainmentState.NovelResumeRequest) && _isPaused)
                {
                    _isPaused = false;
                    _flipTimer?.Start();
                    UpdateDisplay();
                }
                else if (name == nameof(EntertainmentState.NovelNextPageRequest))
                {
                    FlipPage(+1);
                }
                else if (name == nameof(EntertainmentState.NovelPrevPageRequest))
                {
                    FlipPage(-1);
                }
            };
        }

        LoadNovel(Settings.NovelFilePath);

        _flipTimer = new System.Timers.Timer(Math.Max(1, Settings.NovelFlipIntervalSeconds) * 1000);
        _flipTimer.Elapsed += (_, _) => Dispatcher.UIThread.InvokeAsync(() => FlipPage(+1));
        _flipTimer.Start();

        Settings.PropertyChanged += (s, a) =>
        {
            var name = a.PropertyName;
            if (name == nameof(NovelReaderSettings.NovelFilePath))
            {
                SaveProgress();
                LoadNovel(Settings.NovelFilePath);
            }
            if (name == nameof(NovelReaderSettings.NovelFlipIntervalSeconds) && _flipTimer != null)
                _flipTimer.Interval = Math.Max(1, Settings.NovelFlipIntervalSeconds) * 1000;
        };

        // 点击组件切换暂停
        RootPanel.PointerPressed += (_, _) => { TogglePause(); };

        UpdateDisplay();
    }

    protected override void OnDetachedFromVisualTree(Avalonia.VisualTreeAttachmentEventArgs e)
    {
        SaveProgress();
        _flipTimer?.Stop(); _flipTimer?.Dispose(); _flipTimer = null;
        base.OnDetachedFromVisualTree(e);
    }

    private void LoadNovel(string path)
    {
        _novelText = ""; _flatText = ""; _currentCharIndex = 0;
        try
        {
            if (File.Exists(path))
            {
                _novelText = File.ReadAllText(path);
                _flatText = System.Text.RegularExpressions.Regex.Replace(_novelText, @"\s+", " ");
                _currentCharIndex = LoadProgress();
                Logger.Info($"小说加载: {path} ({_flatText.Length}字符, 进度{_currentCharIndex})");
            }
            else Logger.Warn($"文件不存在: {path}");
        }
        catch (Exception ex) { Logger.Error($"加载失败: {ex.Message}"); }
        UpdateDisplay();
    }

    public void FlipPage(int direction)
    {
        var cpp = Math.Max(10, Settings.NovelCharsPerPage);
        if (_flatText.Length == 0) return;
        int max = _flatText.Length;
        _currentCharIndex = direction > 0
            ? Math.Min(_currentCharIndex + cpp, max)
            : Math.Max(0, _currentCharIndex - cpp);
        SaveProgress();
        UpdateDisplay();
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            _flipTimer?.Stop();
        }
        else
        {
            _flipTimer?.Start();
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (_flatText.Length == 0)
        {
            PageContent.Content = new TextBlock { Text = "请先在组件设置中选择小说文件" };
            PageInfo.Text = "";
            return;
        }
        var cpp = Math.Max(10, Settings.NovelCharsPerPage);
        var end = Math.Min(_currentCharIndex + cpp, _flatText.Length);
        PageContent.Content = new TextBlock
        {
            Text = _flatText[_currentCharIndex..end].Trim(),
            TextTrimming = TextTrimming.CharacterEllipsis
        };
        var total = Math.Max(1, (int)Math.Ceiling((double)_flatText.Length / cpp));
        PageInfo.Text = $"第 {Math.Min(_currentCharIndex / cpp + 1, total)}/{total} 页  ·  {(int)(_currentCharIndex * 100.0 / _flatText.Length)}%{(_isPaused ? "  ⏸" : "")}";
    }

    // ===== 进度保存/恢复 =====
    private static string ProgFile(string novelPath) =>
        Path.Combine(Path.GetDirectoryName(novelPath) ?? ".",
            "." + Path.GetFileNameWithoutExtension(novelPath) + ".novelpos");

    private void SaveProgress()
    {
        if (string.IsNullOrEmpty(Settings.NovelFilePath) || !File.Exists(Settings.NovelFilePath)) return;
        try
        {
            Settings.SavedPosition = _currentCharIndex;
            File.WriteAllText(ProgFile(Settings.NovelFilePath), _currentCharIndex.ToString());
        }
        catch { }
    }

    private int LoadProgress()
    {
        if (string.IsNullOrEmpty(Settings.NovelFilePath) || !File.Exists(Settings.NovelFilePath)) return 0;
        try
        {
            var pf = ProgFile(Settings.NovelFilePath);
            if (File.Exists(pf) && int.TryParse(File.ReadAllText(pf), out var pos))
                return pos;
        }
        catch { }
        return 0;
    }
}
