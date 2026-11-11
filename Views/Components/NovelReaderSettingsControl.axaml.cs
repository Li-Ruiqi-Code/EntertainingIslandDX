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

using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ClassIsland.Core;
using ClassIsland.Core.Abstractions.Controls;
using ClassIsland.Shared;
using EntertainingIsland.Models;
using EntertainingIsland.Services;

namespace EntertainingIsland.Views.Components;

public partial class NovelReaderSettingsControl : ComponentBase<NovelReaderSettings>
{
    public NovelReaderSettingsControl()
    {
        InitializeComponent();
        BrowseBtn.Click += async (_, _) =>
        {
            var window = TopLevel.GetTopLevel(this) ?? AppBase.Current.GetRootWindow();
            var files = await window.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "选择小说文件",
                AllowMultiple = false
            });
            if (files.Count > 0)
                Settings.NovelFilePath = files[0].Path.LocalPath;
        };

        RestartBtn.Click += (_, _) =>
        {
            // 通过 EntertainmentState 发送重新开始信号
            var state = IAppHost.TryGetService<EntertainmentState>();
            if (state != null)
            {
                // 用 NovelRestartRequest 标记触发重新开始
                state.NovelRestartRequest = !state.NovelRestartRequest;
            }
        };
    }
}
