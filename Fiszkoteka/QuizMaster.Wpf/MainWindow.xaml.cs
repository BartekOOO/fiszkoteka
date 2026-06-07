using Microsoft.Extensions.DependencyInjection;
using QuizMaster.Contracts.Auth;
using QuizMaster.Wpf.Interfaces;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMaster.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAppSession _appSession;
        private readonly IAuthApiClient _authApiClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMessageDialogService _messageDialogService;

        public MainWindow(
            IAppSession appSession,
            IServiceProvider serviceProvider,
            IMessageDialogService messageDialogService,
            IAuthApiClient authApiClient)
        {
            InitializeComponent();

            _appSession = appSession;
            _serviceProvider = serviceProvider;

            LoadUserInfo();
            _messageDialogService = messageDialogService;
            _authApiClient = authApiClient;
        }

        private void LoadUserInfo()
        {
            UserNameTextBlock.Text = string.IsNullOrWhiteSpace(_appSession.UserName)
                ? "Użytkownik"
                : _appSession.UserName;

            UserEmailTextBlock.Text = string.IsNullOrWhiteSpace(_appSession.Email)
                ? "-"
                : _appSession.Email;
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            _appSession.Clear();

            var logout = new LogoutRequest()
            {
                Token = _appSession.Token
            };

            try
            {
                await _authApiClient.Logout(logout);

                var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
                loginWindow.Show();

                Close();
            }
            catch(Exception ex)
            {
                _messageDialogService.ShowError($"Błąd", ex.Message, this);
            }
        }
    }
}