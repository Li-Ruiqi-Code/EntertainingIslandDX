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
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EntertainingIsland.Models;

namespace EntertainingIsland.Services;

/// <summary>
/// 每日运势服务。每天用日期做种子随机抽取宜/忌条目。
/// </summary>
public partial class FortuneService : ObservableObject
{
    private readonly FortuneSettings _settings;

    /// <summary>今日宜</summary>
    [ObservableProperty]
    private ObservableCollection<FortuneEntry> _todayGood = new();

    /// <summary>今日忌</summary>
    [ObservableProperty]
    private ObservableCollection<FortuneEntry> _todayBad = new();

    public FortuneService(FortuneSettings settings)
    {
        _settings = settings;
        RefreshToday();

        // 设置变更时自动刷新
        _settings.PropertyChanged += (_, a) =>
        {
            if (a.PropertyName is nameof(FortuneSettings.GoodCount) or nameof(FortuneSettings.BadCount))
                RefreshToday();
        };
        _settings.GoodFortunes.CollectionChanged += (_, _) => RefreshToday();
        _settings.BadFortunes.CollectionChanged += (_, _) => RefreshToday();
    }

    /// <summary>
    /// 刷新今日运势。默认按日期种子，手动刷新时强制随机。
    /// </summary>
    public void RefreshToday(bool forceRandom = false)
    {
        if (_settings.GoodFortunes.Count == 0 || _settings.BadFortunes.Count == 0)
            return;

        var seed = DateTime.Today.Year * 10000 + DateTime.Today.Month * 100 + DateTime.Today.Day;
        var rng = forceRandom ? new Random() : new Random(seed);

        var good = PickRandom(_settings.GoodFortunes.ToList(), _settings.GoodCount, rng);
        var bad = PickRandom(_settings.BadFortunes.ToList(), _settings.BadCount, rng);

        TodayGood = new ObservableCollection<FortuneEntry>(good);
        TodayBad = new ObservableCollection<FortuneEntry>(bad);
    }

    private static List<FortuneEntry> PickRandom(List<FortuneEntry> pool, int count, Random rng)
    {
        // Fisher-Yates 洗牌后取前 count 个
        var shuffled = pool.ToList();
        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }
        return shuffled.Take(Math.Min(count, shuffled.Count)).ToList();
    }
}
