using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            var builder = Host.CreateApplicationBuilder();

            builder.Logging.AddConsole();

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

            logger.LogInformation("ContainerLoadingService.GetDraft() called.");

            host.Run();
        }
    }
}
