using QuizMaster.Contracts.Auth;
using QuizMaster.Wpf.Delegates;
using QuizMaster.Wpf.Interfaces;
using System;
using System.Collections.Generic;
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
    /// Logika interakcji dla klasy RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly IAuthApiClient _authApiClient;
        private readonly IMessageDialogService _messageDialogService;
        public event RegistrationFinishedHandler Finished;
        public RegisterWindow(IAuthApiClient authApiClient, IMessageDialogService messageDialogService)
        {
            InitializeComponent();
            _authApiClient = authApiClient;
            _messageDialogService = messageDialogService;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            var email = EmailTextBox.Text;
            var password = PasswordBox.Password;
            var repeatedPassword = RepeatPasswordBox.Password;

            RegisterButton.IsEnabled = false;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Podaj nazwę użytkownika.");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ShowError("Podaj adres e-mail.");
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Podaj hasło.");
                return;
            }

            if (password != repeatedPassword)
            {
                ShowError("Hasła nie są takie same.");
                return;
            }

            try
            {
                var request = new RegisterRequest()
                {
                    Email = email,
                    UserName = username,
                    Password = password
                };

                _ = await _authApiClient.RegisterAsync(request);

                _messageDialogService.ShowSuccess("Komunikat", 
                    "Pomyślnie zarejestrowano nowego użytkownika");

                this.Finished?.Invoke(email, password);
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            RegisterButton.IsEnabled = true;
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Finished(null, null);
        }

        private void ShowError(string message)
        {
            _messageDialogService.ShowError("Błąd", message, this);
        }
    }
}
