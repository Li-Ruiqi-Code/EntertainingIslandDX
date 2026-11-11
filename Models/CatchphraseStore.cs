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
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EntertainingIsland.Models;

/// <summary>
/// 口头禅存储
/// </summary>
public partial class CatchphraseStore : ObservableObject
{
    private readonly Dictionary<string, int> _phrases = new(StringComparer.OrdinalIgnoreCase);
    private readonly string _savePath;

    public CatchphraseStore(string pluginConfigFolder)
    {
        _savePath = Path.Combine(pluginConfigFolder, "Catchphrases.json");
        Load();
    }

    public IReadOnlyDictionary<string, int> Phrases => _phrases;

    public void Add(string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase)) return;
        var key = phrase.Trim();
        if (_phrases.ContainsKey(key))
            _phrases[key]++;
        else
            _phrases[key] = 1;
        OnPropertyChanged(nameof(Phrases));
        Save();
    }

    public void Clear()
    {
        _phrases.Clear();
        OnPropertyChanged(nameof(Phrases));
        Save();
    }

    public string GetDisplayText()
    {
        if (_phrases.Count == 0) return "暂无口头禅记录";
        var parts = new List<string>();
        foreach (var (phrase, count) in _phrases)
            parts.Add($"{phrase} x{count}");
        return string.Join("  ", parts);
    }

    private void Save()
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(_phrases);
            File.WriteAllText(_savePath, json);
        }
        catch { }
    }

    private void Load()
    {
        try
        {
            if (File.Exists(_savePath))
            {
                var json = File.ReadAllText(_savePath);
                var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(json);
                if (dict != null)
                    foreach (var (k, v) in dict) _phrases[k] = v;
            }
        }
        catch { }
    }
}
