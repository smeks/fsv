using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Economy.Client;
using Economy.Domain;
using Economy.Domain.IManager;
using Economy.FSX;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Economy.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        App()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            MainWindow = mainWindow;

            // set window handle on managers that need it
            var handle = Process.GetCurrentProcess().MainWindowHandle;
            ServiceProvider.GetService<ISimConnectManager>().WindowHandle = handle;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(MainWindow));
            services.AddSingleton<IFlightManager, FlightManager>();
            services.AddSingleton<ISimConnectManager, SimConnectManager>();
            services.AddSingleton<IEconomyClient, EconomyClient>();
            services.AddSingleton<IIcaoManager, IcaoManager>();
        }
    }
}
