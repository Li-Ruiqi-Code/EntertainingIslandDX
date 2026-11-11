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

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EntertainingIsland.Models;

/// <summary>
/// RSS 新闻组件设置
/// </summary>
public partial class RssComponentSettings : ObservableObject
{
    [ObservableProperty]
    private string _rssFeedUrl = "https://www.zhihu.com/rss";

    [ObservableProperty]
    private int _rssDisplayCount = 1;

    [ObservableProperty]
    private int _rssFlipIntervalSeconds = 8;

    [ObservableProperty]
    private int _rssMaxTitleLength = 50;
}

/// <summary>
/// RSS 预设源
/// </summary>
public static class RssPresets
{
    public static ReadOnlyCollection<(string Name, string Url)> Feeds { get; } = new(
        new List<(string, string)>
        {
            ("少数派", "https://sspai.com/feed"),
            ("V2EX 最新", "https://www.v2ex.com/feed/tab/tech.xml"),
            ("Hacker News", "https://news.ycombinator.com/rss"),
            ("36氪", "https://36kr.com/feed"),
            ("TechCrunch", "http://feeds.feedburner.com/TechCrunch/"),
            ("The Verge", "https://www.theverge.com/rss/index.xml"),
            ("Ars Technica", "http://feeds.arstechnica.com/arstechnica/index/"),
        }
    );
}
