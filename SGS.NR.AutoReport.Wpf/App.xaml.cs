using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SGS.NR.AutoReport.Wpf.Services;
using SGS.NR.AutoReport.Wpf.ViewModels;
using System.IO;
using System.Windows;
using SGS.NR.Util.Helper;
using SGS.NR.Util.Extension;

namespace SGS.NR.AutoReport.Wpf
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                // 載入設定檔 (必須存在，執行期間修改會重載)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                // 載入環境設定檔 (非必要，適合部署人員搭配使用)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true);

            // 設定 Serilog
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) // 讀取設定檔中的日誌設定
                .Enrich.WithProperty("Version", VersionHelper.CurrentVersion) // 版本無法寫在設定檔
                .CreateLogger();

            try
            {
                Log.Information("{Application} {Version} 啟動");

                // 清除預設的日誌提供者
                builder.Logging.ClearProviders();
                // 使用 Serilog 取代內建的日誌機制
                builder.Logging.AddSerilog();

                builder.Services.AddSingleton(p => new MainWindow
                {
                    DataContext = p.GetRequiredService<MainViewModel>()
                });

                builder.Services.AddSingleton<MainViewModel>();

                builder.Services.AddSingleton<IDialogService, DialogService>();

                _host = builder.Build();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }

}
