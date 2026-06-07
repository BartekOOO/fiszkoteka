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
        private readonly IAppSettings _appSettings;

        public LoginWindow(
            IAuthApiClient authApiClient,
            IMessageDialogService messageDialogService,
            IServiceProvider serviceProvider,
            IAppSession appSession,
            IAppSettings appSettings)
        {
            InitializeComponent();

            _authApiClient = authApiClient;
            _messageDialogService = messageDialogService;
            _serviceProvider = serviceProvider;
            _appSession = appSession;
            _appSettings = appSettings;

            _appSettings.Load();

            if (_appSettings.RememberLogin)
                EmailTextBox.Text = _appSettings.SavedEmail;

            RememberMeCheckbox.IsChecked = _appSettings.RememberLogin;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;

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

                _appSettings.SaveRememberLogin(EmailTextBox.Text, RememberMeCheckbox.IsChecked ?? false);

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

            LoginButton.IsEnabled = true;
        }

        private void RegisterAccount_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = _serviceProvider.GetRequiredService
                <RegisterWindow>();

            registerWindow.Finished += (l, p) =>
            {
                EmailTextBox.Text = l;
                PasswordBox.Password = p;
                registerWindow.Close();
            };

            registerWindow.ShowDialog();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(LoginButton, new RoutedEventArgs());
            }
        }
    }
}
