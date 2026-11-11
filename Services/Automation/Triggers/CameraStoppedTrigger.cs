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

using ClassIsland.Core.Abstractions.Automation;
using ClassIsland.Core.Attributes;
using ClassIsland.Shared;
using EntertainingIsland.Services;

namespace EntertainingIsland.Services.Automation.Triggers;

[TriggerInfo("entertainingisland.camera.stopped", "摄像头被关闭时", "\uE392")]
public class CameraStoppedTrigger : TriggerBase
{
    private CameraMonitorService? _service;

    public override void Loaded()
    {
        _service = IAppHost.TryGetService<CameraMonitorService>();
        if (_service != null)
            _service.PropertyChanged += OnCameraStatusChanged;
    }

    public override void UnLoaded()
    {
        if (_service != null)
            _service.PropertyChanged -= OnCameraStatusChanged;
    }

    private void OnCameraStatusChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CameraMonitorService.IsCameraInUse) && _service?.IsCameraInUse == false)
            Trigger();
    }
}
