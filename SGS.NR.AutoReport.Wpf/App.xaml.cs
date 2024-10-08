using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace SGS.NR.AutoReport.Wpf
{
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddSingleton<MainWindow>();

            _host = builder.Build();
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
