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

using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Core.Attributes;
using ClassIsland.Shared;
using EntertainingIsland.Models;
using EntertainingIsland.Services;

namespace EntertainingIsland.Views.Components;

[ComponentInfo(
    "9F954BE3-4CAF-440D-AD3B-03035D0059BF",
    "每日运势",
    "\uEFFF",
    "每日随机宜忌运势，高中生专属趣味运势指南")]
public partial class FortuneComponent : ComponentBase<FortuneComponentSettings>
{
    private FortuneService? _service;

    public FortuneComponent()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        var state = IAppHost.TryGetService<EntertainmentState>();
        if (state != null)
            state.PropertyChanged += (_, a) =>
            {
                if (a.PropertyName == nameof(EntertainmentState.IsHidden))
                    IsVisible = !state.IsHidden;
            };

        _service = IAppHost.TryGetService<FortuneService>();
        if (_service != null)
        {
            _service.PropertyChanged += (_, a) =>
            {
                if (a.PropertyName is nameof(FortuneService.TodayGood) or nameof(FortuneService.TodayBad))
                    Dispatcher.UIThread.Post(RefreshDisplay);
            };
        }

        RefreshDisplay();
    }

    private void RefreshDisplay()
    {
        if (_service == null) return;

        var good = _service.TodayGood;
        var bad = _service.TodayBad;

        GoodLine.Text = good.Count > 0
            ? "宜 " + string.Join("  ", good.Select(g => $"{g.MainText} · {g.SubText}"))
            : "宜 ——";

        var showOnlyGood = IAppHost.TryGetService<Plugin>()?.Settings.Fortune.ShowOnlyGood == true;
        BadLine.IsVisible = !showOnlyGood;
        BadLine.Text = bad.Count > 0
            ? "忌 " + string.Join("  ", bad.Select(b => $"{b.MainText} · {b.SubText}"))
            : "忌 ——";
    }
}
