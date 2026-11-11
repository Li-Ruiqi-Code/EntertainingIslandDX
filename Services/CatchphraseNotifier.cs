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
using ClassIsland.Core.Abstractions.Services.NotificationProviders;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Models.Notification;
using Ns = ClassIsland.Shared.Models.Notification;

namespace EntertainingIsland.Services;

/// <summary>
/// 口头禅提醒提供方。老师每说一次口头禅时弹出一秒强调提示。
/// </summary>
[NotificationProviderInfo(
    "C7E3B152-6A4E-4B2F-9D18-2F5B8C41D7E3",
    "口头禅提醒",
    "\uE3E4",
    "老师每说一次口头禅时弹出强调提示。"
)]
public class CatchphraseNotifier : NotificationProviderBase
{
    /// <summary>
    /// 显示一次口头禅强调提醒。
    /// </summary>
    public void ShowEmphasis(string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase)) return;

        try
        {
            var request = new NotificationRequest
            {
                MaskContent = NotificationContent.CreateTwoIconsMask(
                    phrase,
                    rightIcon: "\uE3E4",
                    factory: x => x.Duration = TimeSpan.FromSeconds(1)),
                RequestNotificationSettings = new Ns.NotificationSettings
                {
                    IsSettingsEnabled = false,
                    IsNotificationEnabled = true,
                    IsNotificationEffectEnabled = true,
                    IsNotificationTopmostEnabled = true
                }
            };

            ShowNotification(request);
        }
        catch (Exception ex)
        {
            Logger.Error($"[口头禅提醒] 发送失败: {ex.Message}");
        }
    }
}
