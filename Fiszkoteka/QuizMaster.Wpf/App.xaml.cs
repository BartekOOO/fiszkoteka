using Microsoft.Extensions.DependencyInjection;
using QuizMaster.Wpf.Interfaces;
using QuizMaster.Wpf.Services;
using QuizMaster.Wpf.Views;
using QuizMaster.Wpf.Windows;
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
            MainWindow = loginWindow;
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IMessageDialogService, MessageDialogService>();

            services.AddHttpClient<IAuthApiClient, AuthApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7237");
            });

            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7237");
            });

            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
            services.AddTransient<RegisterWindow>();
            services.AddTransient<DashboardView>();
            services.AddTransient<FlashcardSetsView>();
            services.AddTransient<LearningProgressView>();
            services.AddTransient<EditFlashcardSetWindow>();
            services.AddTransient<CreateFlashcardSetWindow>(sp =>
            {
                var window = new CreateFlashcardSetWindow(
                    sp.GetRequiredService<IApiClient>(),
                    sp.GetRequiredService<IMessageDialogService>());

                window.OnCreatedFlashcardSet += (sender, id) =>
                {
                    var editWindow = sp.GetRequiredService<EditFlashcardSetWindow>();

                    if (sender is CreateFlashcardSetWindow createWindow)
                    {
                        editWindow.Owner = createWindow.Owner;
                        editWindow.Show();
                        createWindow.Close();
                    }
                };

                return window;
            });

            services.AddTransient<SettingsView>();
            services.AddSingleton<IAppSession, AppSession>();
            services.AddSingleton<IAppSettings, AppSettings>();
            services.AddSingleton<SessionEvents>();
            services.AddSingleton<ISessionEvents>(sp =>
            {
                var sessionEvents = sp.GetRequiredService<SessionEvents>();
                return sessionEvents;
            });
        }
    }

}
