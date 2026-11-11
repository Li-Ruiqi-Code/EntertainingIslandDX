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

using System.Windows.Input;
using ClassIsland.Core.Abstractions.Controls;
using CommunityToolkit.Mvvm.Input;
using EntertainingIsland.Models;
using EntertainingIsland.Services;

namespace EntertainingIsland.Views.NotificationProviders;

public partial class ClassEndingReminderSettingsControl : NotificationProviderControlBase<ClassEndingReminderSettings>
{
    public ICommand DebugPreviewCommand { get; }

    public ClassEndingReminderSettingsControl()
    {
        InitializeComponent();
        DataContext = this;

        DebugPreviewCommand = new RelayCommand(() =>
        {
            ClassEndingReminderService.Instance?.DebugTriggerPreview();
        });
    }
}
