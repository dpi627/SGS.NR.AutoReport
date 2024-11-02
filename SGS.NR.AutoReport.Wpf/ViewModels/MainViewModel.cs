using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SGS.NR.AutoReport.Wpf.Models;
using SGS.NR.Util.Helper;
using System.Windows.Controls;

namespace SGS.NR.AutoReport.Wpf.ViewModels;

public partial class MainViewModel : ObservableObject
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

    private readonly AppSettings _appSettings;

    public IAsyncRelayCommand NavigateToLabelPrintAsyncCommand { get; }
    public IAsyncRelayCommand NavigateToExcelConfigAsyncCommand { get; }

    public MainViewModel(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;

        NavigateToLabelPrintAsyncCommand = new AsyncRelayCommand(NavigateToLabelPrintAsync);
        NavigateToExcelConfigAsyncCommand = new AsyncRelayCommand(NavigateToExcelConfigAsync);
        WindowTitle = $"{AppDomain.CurrentDomain.FriendlyName} - {VersionHelper.CurrentVersion}";
        AppTitle = _appSettings.AppTitle;

        Task.Run(() => NavigateToLabelPrintAsync());
    }

    [RelayCommand]
    private void NavigateToLabelPrint()
    {
        //CurrentPage = App.AppHost!.Services.GetRequiredService<LabelPrintPage>();
        CloseLeftDrawer();
    }

    [RelayCommand]
    private void NavigateToExcelConfig()
    {
        //CurrentPage = App.AppHost!.Services.GetRequiredService<ExcelConfigPage>();
        CloseLeftDrawer();
    }

    private void CloseLeftDrawer()
    {
        IsLeftDrawerOpen = false;
    }

    private async Task NavigateToLabelPrintAsync()
    {
        //await NavigateToPageAsync<LabelPrintPage>();
    }

    private async Task NavigateToExcelConfigAsync()
    {
        //await NavigateToPageAsync<ExcelConfigPage>();
    }

    private async Task NavigateToPageAsync<T>() where T : Page
    {
        CurrentPage = null;
        IsLoading = true;
        IsLeftDrawerOpen = false;

        //await Task.Yield();
        await Task.Delay(500);

        try
        {
            // 在背景線程創建頁面實例
            await Task.Run(() =>
            {

            });

            // 在UI線程上設置 CurrentPage
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                //var page = App.AppHost!.Services.GetRequiredService<T>();
                //CurrentPage = page;
            });
        }
        finally
        {
            // 確保在UI線程上設置 IsLoading = false
            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                IsLoading = false;
            });
        }
    }
}
