using CommunityToolkit.Mvvm.ComponentModel;

namespace EntertainingIsland.ViewModels;

public partial class WelcomeViewModel : ObservableObject
{
    [ObservableProperty]
    private int _slideIndex = 0;

    [ObservableProperty]
    private Type? _currentPage;

    [ObservableProperty]
    private bool _enableNovelReader = true;

    [ObservableProperty]
    private bool _enableFortune = true;

    [ObservableProperty]
    private bool _enableCatchphrase = true;

    [ObservableProperty]
    private bool _enableSports = true;

    [ObservableProperty]
    private bool _enableCameraStatus = true;

    [ObservableProperty]
    private bool _isWizardCompleted = false;
}
