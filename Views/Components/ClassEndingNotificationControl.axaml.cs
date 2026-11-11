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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ClassIsland.Core.Abstractions.Services;
using ClassIsland.Shared;

namespace EntertainingIsland.Views.Components;

public partial class ClassEndingNotificationControl : UserControl, INotifyPropertyChanged
{
    private object? _element;
    private string _message = "";
    private int _slideIndex;
    private bool _showTeacherName;
    private string _nextSubjectName = "";
    private string _nextSubjectTeacherName = "";
    private TimeSpan _nextClassStartTime;
    private TimeSpan _nextClassEndTime;

    public object? Element
    {
        get => _element;
        set
        {
            if (Equals(value, _element)) return;
            _element = value;
            OnPropertyChanged();
        }
    }

    public string Message
    {
        get => _message;
        set
        {
            if (value == _message) return;
            _message = value;
            OnPropertyChanged();
        }
    }

    public int SlideIndex
    {
        get => _slideIndex;
        set
        {
            if (value == _slideIndex) return;
            _slideIndex = value;
            OnPropertyChanged();
        }
    }

    public bool ShowTeacherName
    {
        get => _showTeacherName;
        set
        {
            if (value == _showTeacherName) return;
            _showTeacherName = value;
            OnPropertyChanged();
        }
    }

    /// <summary>下节课名称（由 ClassEndingReminderService 计算后设置）</summary>
    public string NextSubjectName
    {
        get => _nextSubjectName;
        set { if (value == _nextSubjectName) return; _nextSubjectName = value; OnPropertyChanged(); }
    }

    /// <summary>下节课教师名</summary>
    public string NextSubjectTeacherName
    {
        get => _nextSubjectTeacherName;
        set { if (value == _nextSubjectTeacherName) return; _nextSubjectTeacherName = value; OnPropertyChanged(); }
    }

    /// <summary>下节课开始时间</summary>
    public TimeSpan NextClassStartTime
    {
        get => _nextClassStartTime;
        set { if (value == _nextClassStartTime) return; _nextClassStartTime = value; OnPropertyChanged(); }
    }

    /// <summary>下节课结束时间</summary>
    public TimeSpan NextClassEndTime
    {
        get => _nextClassEndTime;
        set { if (value == _nextClassEndTime) return; _nextClassEndTime = value; OnPropertyChanged(); }
    }

    public ILessonsService LessonsService { get; } = IAppHost.GetService<ILessonsService>();

    private DispatcherTimer Timer { get; } = new()
    {
        Interval = TimeSpan.FromSeconds(10)
    };

    public ClassEndingNotificationControl()
    {
        InitializeComponent();
        Element = this.FindResource("ClassEndingOverlay") as Control;
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        MainListBox.SelectedIndex = 0;
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        Timer.Start();
        Timer.Tick += TimerOnTick;
        MainListBox.SelectedIndex = SlideIndex;
    }

    private void OnUnloaded(object? o, RoutedEventArgs routedEventArgs)
    {
        Timer.Stop();
        Timer.Tick -= TimerOnTick;
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Message))
            return;
        MainListBox.SelectedIndex = SlideIndex = SlideIndex == 1 ? 0 : 1;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
