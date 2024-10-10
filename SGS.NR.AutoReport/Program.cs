using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SGS.NR.AutoReport.Extension;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.Interface;

namespace SGS.NR.AutoReport
{
    internal class Program
    {
        static void Main()
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
                .CreateLogger();

            try
            {
                Log.Information("應用程式啟動中");

                // 清除預設的日誌提供者
                builder.Logging.ClearProviders();
                // 使用 Serilog 取代內建的日誌機制
                builder.Logging.AddSerilog();

                // 註冊服務
                builder.Services.AddServices()
                    .AddRepositories()
                    .AddMiscs();

                var host = builder.Build();

                // 取得 Configuration 服務
                //var config = host.Services.GetRequiredService<IConfiguration>();
                // 讀取特定 section 並轉換為強型別
                //var containerLoading = config.GetSection("ContainerLoading").Get<ContainerLoading>();
                // 取得 ContainerLoading 服務
                var serviceCL = host.Services.GetRequiredService<IContainerLoadingService>();
                // 建立 ContainerLoadingInfo 物件
                var infoCL = new ContainerLoadingInfo()
                {
                    SourcePath = @"C:\dev\SGS.NR.AutoReport\Doc\0.BU\裝櫃電子表單1006.xlsm",
                    TemplatePath = @"Templates\Draft.Container.Loading.docx",
                    TargetPath = $@"C:\dev\_tmp\DCL{DateTime.Now:yyyyMMddHHmmss}.docx"
                };
                // 呼叫 ContainerLoadingService 服務
                var resultCL = serviceCL.GetDraft(infoCL);

                // 取得 VesselLoading 服務
                var service = host.Services.GetRequiredService<IVesselLoadingService>();
                // 建立 Info
                var info = new VesselLoadingInfo()
                {
                    SourcePath = @"C:\dev\SGS.NR.AutoReport\Doc\0.BU\裝船電子表單1006.xlsm",
                    TemplatePath = @"Templates\Draft.Vessel.Loading.docx",
                    TargetPath = $@"C:\dev\_tmp\DVL{DateTime.Now:yyyyMMddHHmmss}.docx"
                };
                // 製作草稿
                var result = service.GetDraft(info);

                //host.Run(); // 這個方法會一直等待，直到應用程式結束 (非 hosting service 可不用執行)
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
