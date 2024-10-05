using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SGS.NR.AutoReport.DTOs;
using SGS.NR.AutoReport.Extension;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.Interface;

namespace SGS.NR.AutoReport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 設定 Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()  // 設定最低日誌層級
                .WriteTo.Console()     // 輸出到 Console
                .WriteTo.File("log/log-.txt", rollingInterval: RollingInterval.Day) // 每天寫入檔案
                .WriteTo.Seq("http://twtpeoad002:5341/")  // 將日誌寫入到 Seq（請確認 Seq 的 URL）
                .CreateLogger();

            try
            {
                Log.Information("應用程式啟動中");

                var builder = Host.CreateApplicationBuilder();

                // 使用 Serilog 取代內建的日誌機制
                builder.Logging.ClearProviders(); // 清除預設的日誌提供者
                builder.Logging.AddSerilog();

                builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                builder.Services.AddOptions<ContainerLoading>().BindConfiguration("appsettings.json");

                builder.Services.AddServices()
                    .AddRepositories()
                    .AddMiscs();

                var host = builder.Build();

                // 取得 Logger 服務
                var logger = host.Services.GetRequiredService<ILogger<Program>>();

                // 取得 Configuration 服務
                var config = host.Services.GetRequiredService<IConfiguration>();
                // 讀取特定 section 並轉換為強型別
                var containerLoading = config.GetSection("ContainerLoading").Get<ContainerLoading>();
                // 取得 ContainerLoading 服務
                var service = host.Services.GetRequiredService<IContainerLoadingService>();
                // 建立 ContainerLoadingInfo 物件
                var info = new ContainerLoadingInfo()
                {
                    SourcePath = containerLoading.SourcePath,
                    TemplatePath = containerLoading.TemplatePath,
                    TargetPath = string.Format(containerLoading.TargetPath, DateTime.Now.ToString("yyyyMMddHHmmss"))
                };
                // 呼叫 ContainerLoadingService 服務
                var result = service.GetDraft(info);

                Log.Information("Export {@info}", info);
                Log.Information("Create {TargetPath}", Path.GetFileName(info.TargetPath));

                host.Run();
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
    }
}
