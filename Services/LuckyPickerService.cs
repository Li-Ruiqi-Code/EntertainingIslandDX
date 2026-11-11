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
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EntertainingIsland.Models;

namespace EntertainingIsland.Services;

/// <summary>
/// 点名器核心服务。每次点名都是独立的均匀随机抽样，不记历史、不加权、无保底。
/// </summary>
public partial class LuckyPickerService : ObservableObject
{
    private readonly LuckyPickerSettings _settings;
    private readonly Random _rng = new();

    [ObservableProperty]
    private List<string> _names = new();

    [ObservableProperty]
    private string _lastPicked = "";

    /// <summary>最近一次被抽中的人的索引</summary>
    public int LastPickedIndex { get; private set; } = -1;

    public LuckyPickerService(LuckyPickerSettings settings)
    {
        _settings = settings;
        ParseNames();
        _settings.PropertyChanged += (_, a) =>
        {
            if (a.PropertyName == nameof(LuckyPickerSettings.NameListText))
                ParseNames();
        };
    }

    /// <summary>
    /// 解析名单元文本为列表
    /// </summary>
    public void ParseNames()
    {
        var text = _settings.NameListText ?? "";
        Names = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => n.Trim())
                    .Where(n => n.Length > 0)
                    .ToList();
    }

    /// <summary>
    /// 随机抽取一个姓名。每次调用都是独立的均匀随机，与上一次结果无关。
    /// </summary>
    public string Pick()
    {
        if (Names.Count == 0)
            return "(名单为空)";

        var pickedIndex = _rng.Next(Names.Count);
        LastPickedIndex = pickedIndex;
        LastPicked = Names[pickedIndex];
        return LastPicked;
    }

    /// <summary>
    /// 获取显示文本（"公正点名"风格）
    /// </summary>
    public string GetDisplayText()
    {
        if (Names.Count == 0)
            return "请先在设置中导入名单";
        if (!string.IsNullOrEmpty(LastPicked))
            return $"{LastPicked}";
        return $"点名 · {Names.Count} 人";
    }

    /// <summary>
    /// 随机抽取两人（不重复）。同样是一次性均匀随机：先洗牌再取前两个。
    /// </summary>
    /// <returns>格式如 "张三、李四"；名单不足时返回提示文本。</returns>
    public string PickTwo()
    {
        if (Names.Count < 2)
            return "(名单不足两人)";

        // Fisher-Yates 洗牌后取前两个，保证两人不重复且组合等概率
        var indices = Enumerable.Range(0, Names.Count).ToArray();
        for (int i = indices.Length - 1; i > 0; i--)
        {
            int j = _rng.Next(i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]);
        }

        var firstIndex = indices[0];
        var secondIndex = indices[1];

        var result = $"{Names[firstIndex]}、{Names[secondIndex]}";
        LastPickedIndex = firstIndex;
        LastPicked = result;
        return result;
    }
}
