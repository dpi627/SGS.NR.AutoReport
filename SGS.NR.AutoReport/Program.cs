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

            // 透過 ServiceProvider 取得服務 IDraftService 之實例
            IDraftService? service = serviceProvider.GetOrCreateService<IDraftService>();

            service.Save(new DraftInfo() {
                ImportPath = @"C:\Users\Brian_Li\OneDrive - SGS\Test\TestRecord_20240403.xlsx",
                ExportPath = @"C:\dev\_tmp\output\certificate.docx",
                TemplatePath = @"Template\certificate.docx"
            });
        }


    }
}
