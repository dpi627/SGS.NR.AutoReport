using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SGS.NR.AutoReport.Wpf.Services;
using SGS.NR.AutoReport.Wpf.ViewModels;
using System.Windows;
using SGS.NR.Util.Helper;
using SGS.NR.AutoReport.Wpf.Models;
using SGS.NR.AutoReport.Wpf.Pages;
using System.Windows.Controls;
using SGS.NR.AutoReport.Wpf.Extensions;
using CommunityToolkit.Mvvm.Messaging;

namespace SGS.NR.AutoReport.Wpf
{
    public partial class App : Application
    {
        private static string? _environment;
        private static string? _appName;
        private static readonly string? _version = VersionHelper.CurrentVersion;
        private static IHost? _host;

        public App()
        {
            var builder = Host.CreateApplicationBuilder();

            _environment = builder.Environment.EnvironmentName;
            _appName = builder.Environment.ApplicationName;

            // 設定組態檔
            builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{_environment}.json", optional: true, reloadOnChange: true);

            // 設定 Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) // 讀取設定檔中的日誌設定
                .Enrich.WithProperty("Version", _version) // 版本無法寫在設定檔
                .Enrich.WithProperty("Application", _appName) //應用程式名稱不寫於設定檔
                .CreateLogger();

            try
            {
                Log.Information("{Application} {Version} 於 {EnvironmentName} 啟動", _appName, _version, _environment);

                // 清除預設的日誌提供者
                builder.Logging.ClearProviders();
                // 使用 Serilog 取代內建的日誌機制
                builder.Logging.AddSerilog();

                // add config
                builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

                // add services
                builder.Services.AddSingleton<IDialogService, DialogService>();
                builder.Services.AddSingleton<INavigationService, NavigationService>();
                builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

                // add view models
                builder.Services.AddSingleton<MainWindowViewModel>();
                builder.Services.AddTransient<ExportDraftViewModel>();

                // add pages
                builder.Services.AddSingleton(p => new MainWindow
                {
                    DataContext = p.GetRequiredService<MainWindowViewModel>()
                });
                builder.Services.AddTransient(p => new ExportDraftPage
                {
                    DataContext = p.GetRequiredService<ExportDraftViewModel>()
                });

                // 註冊服務
                builder.Services.AddServices()
                    .AddRepositories()
                    .AddMiscs();

                _host = builder.Build();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "{Application} {Version} 異常", _appName, _version);
                throw;
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            // 取得 NavigationService 並設置 Frame
            var navigationService = _host.Services.GetRequiredService<INavigationService>();
            if (navigationService is NavigationService navService)
            {
                // 確保主窗口已初始化並載入 XAML 元素
                mainWindow.Show(); // 顯示主窗口以確保 XAML 元素已載入

                // 使用 Dispatcher 延遲設置 Frame，確保 Frame 已被初始化
                await mainWindow.Dispatcher.InvokeAsync(() =>
                {
                    if (mainWindow.FindName("MainFrame") is Frame frame)
                    {
                        navService.SetFrame(frame);

                        // 註冊頁面
                        navService.Configure(nameof(ExportDraftPage), typeof(ExportDraftPage));
                        //navService.Configure("AnotherPage", typeof(AnotherPage));

                        // 導航到初始頁面
                        navService.NavigateTo(nameof(ExportDraftPage));
                    }
                });
            }

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            Log.Information("{Application} {Version} 關閉");
            using (_host)
            {
                await _host.StopAsync();
            }
            Log.CloseAndFlush(); // 在應用結束時關閉 Serilog
            base.OnExit(e);
        }
    }

}
