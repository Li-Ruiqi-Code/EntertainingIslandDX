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

using System.Threading.Tasks;
using ClassIsland.Core.Abstractions.Automation;
using ClassIsland.Core.Attributes;
using ClassIsland.Shared;
using EntertainingIsland.Services;

namespace EntertainingIsland.Services.Automation;

[ActionInfo("entertainingisland.camera-monitor.disable", "禁用摄像头监控", "\uE392", defaultGroupToMenu: "EntertainingIsland/摄像头监控")]
public class DisableCameraMonitorAction : ActionBase
{
    protected override Task OnInvoke()
    {
        var s = IAppHost.TryGetService<CameraMonitorService>();
        if (s != null) s.Enabled = false;
        return Task.CompletedTask;
    }
}
