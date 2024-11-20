using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using SGS.NR.AutoReport.Wpf.Models;
using SGS.NR.AutoReport.Wpf.Services;
using SGS.NR.Util.Helper;
using System.Windows.Controls;

namespace SGS.NR.AutoReport.Wpf.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private bool _isLeftDrawerOpen;

    [ObservableProperty]
    private Page? _currentPage;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private string? _windowTitle;

    [ObservableProperty]
    private string? _appTitle;

    private readonly INavigationService _navigationService;
    private readonly AppSettings _appSettings;

    public MainWindowViewModel(
        INavigationService navigationService,
        IOptions<AppSettings> appSettings,
        IMessenger messenger)
    {
        _navigationService = navigationService;
        _appSettings = appSettings.Value;
        _messenger = messenger;
        _messenger.Register<LoadingMessage>(this, (r, m) => IsLoading = m.IsLoading);

        WindowTitle = $"{AppDomain.CurrentDomain.FriendlyName} - {VersionHelper.CurrentVersion}";
        AppTitle = _appSettings.AppTitle;
    }

    [RelayCommand]
    private async Task NavigateToExportDraftAsync()
    {
        await NavigateToPageAsync("ExportDraftPage");
    }

    private async Task NavigateToPageAsync(string pageKey)
    {
        CurrentPage = null;
        IsLoading = true;
        IsLeftDrawerOpen = false;

        // 等待 UI 更新
        await Task.Delay(300); // 可調整延遲時間

        try
        {
            // 執行導航
            _navigationService.NavigateTo(pageKey);
        }
        finally
        {
            IsLoading = false;
        }
    }
}
