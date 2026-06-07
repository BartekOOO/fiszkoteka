using Microsoft.Extensions.DependencyInjection;
using QuizMaster.Contracts.Auth;
using QuizMaster.Wpf.Interfaces;
using QuizMaster.Wpf.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuizMaster.Wpf
{
    /// <summary>
    /// Logika interakcji dla klasy LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IAppSession _appSession;

        public LoginWindow(
            IAuthApiClient authApiClient,
            IMessageDialogService messageDialogService,
            IServiceProvider serviceProvider,
            IAppSession appSession)
        {
            InitializeComponent();

            _authApiClient = authApiClient;
            _messageDialogService = messageDialogService;
            _serviceProvider = serviceProvider;
            _appSession = appSession;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                {
                    _messageDialogService.ShowWarning(
                        "Brak adresu email",
                        "Wpisz adres email.",
                        this);

                    return;
                }

                if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    _messageDialogService.ShowWarning(
                        "Brak hasła",
                        "Wpisz hasło.",
                        this);

                    return;
                }

                var request = new LoginRequest
                {
                    Email = EmailTextBox.Text,
                    Password = PasswordBox.Password
                };

                var authResponse = await _authApiClient.LoginAsync(request);

                _appSession.UserId = authResponse.UserId;
                _appSession.UserName = authResponse.UserName;
                _appSession.Email = authResponse.Email;
                _appSession.Token = authResponse.Token;

                var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();

                Close();
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd logowania",
                    ex.Message,
                    this);
            }
        }
    }
}
