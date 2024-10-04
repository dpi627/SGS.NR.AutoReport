using Microsoft.Extensions.DependencyInjection;
using SGS.NR.AutoReport.Extension;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.Interface;

namespace SGS.NR.AutoReport
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 建立 DI 容器，註冊服務
            IServiceCollection services = new ServiceCollection()
                .AddServices()
                .AddRepositories()
                .AddMiscs();
            // 建立 ServiceProvider
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            // 取得特定服務
            var service = serviceProvider.GetOrCreateService<IContainerLoadingService>();
            // 產生草稿
            var result = service.GetDraft(new ContainerLoadingInfo()
            {
                SourcePath = @"C:\dev\_tmp\裝櫃電子表單0401.xlsm",
                TemplatePath = @"Templates\Draft.Container.Loading.docx",
                TargetPath = @$"C:\dev\_tmp\{DateTime.Now:yyyyMMddHHmmss}.docx"
            });
            // 輸出結果
            Console.WriteLine($"{result}");
        }
    }
}
