using Services.EmployeeProviders.GorestV2;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using UI.Factories;
using Services.EmployeeProviders;
using Services.Reporting.ReportGenerators;
using Services.Reporting;
using Services.Parameters.ParameterProviders;
using Services.Parameters;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<IParameterProvider, ConfigurationManagerParameterProvider>();
            services.AddSingleton<IParameterService, ParameterService>();

            services.AddTransient<IEmployeeProvider, GorestV2EmployeeProvider>();
            services.AddTransient<IEmployeeInsertOrEditWindowFactory, EmployeeInsertOrEditWindowFactory>();
            services.AddTransient<IReportGeneratorFactory, ReportGeneratorFactory>();
            services.AddTransient<IReportingService, ReportingService>();

            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();

            mainWindow.Show();
        }
    }
}
