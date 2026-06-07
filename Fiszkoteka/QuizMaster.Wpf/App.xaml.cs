using Microsoft.Extensions.DependencyInjection;
using QuizMaster.Wpf.Interfaces;
using QuizMaster.Wpf.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace QuizMaster.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection();

            ConfigureServices(services);

            Services = services.BuildServiceProvider();

            var loginWindow = Services.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageDialogService, MessageDialogService>();

            services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7237");
            });

            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
            services.AddTransient<RegisterWindow>();
            services.AddSingleton<IAppSession, AppSession>();
            services.AddSingleton<IAppSettings, AppSettings>();
        }
    }

}
