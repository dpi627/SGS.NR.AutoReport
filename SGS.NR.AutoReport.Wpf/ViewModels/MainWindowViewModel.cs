using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using SGS.NR.AutoReport.Wpf.Models;
using SGS.NR.AutoReport.Wpf.Services;
using SGS.NR.Util.Helper;
using System.Windows.Controls;

namespace SGS.NR.AutoReport.Wpf.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
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

    // 非同步RelayCommand 沒有 Attribute 可用：1 要先宣告屬性
    public IAsyncRelayCommand NavigateToExportDraftAsyncCommand { get; }

    public MainWindowViewModel(
        INavigationService navigationService,
        IOptions<AppSettings> appSettings)
    {
        _navigationService = navigationService;
        _appSettings = appSettings.Value;

        WindowTitle = $"{AppDomain.CurrentDomain.FriendlyName} - {VersionHelper.CurrentVersion}";
        AppTitle = _appSettings.AppTitle;

        // 非同步RelayCommand 沒有 Attribute 可用：2 再建構子實體化
        NavigateToExportDraftAsyncCommand = new AsyncRelayCommand(NavigateToExportDraftAsync);

        // 初始頁面
        Task.Run(() => NavigateToExportDraftAsync());
    }

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
