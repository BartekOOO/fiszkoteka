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
        private string _email;
        private string _password;
        public event RegistrationFinishedHandler Finished;
        public RegisterWindow(IAuthApiClient authApiClient, IMessageDialogService messageDialogService)
        {
            InitializeComponent();
            _authApiClient = authApiClient;
            _messageDialogService = messageDialogService;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text;
            _email = EmailTextBox.Text;
            _password = PasswordBox.Password;
            var repeatedPassword = RepeatPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Podaj nazwę użytkownika.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_email))
            {
                ShowError("Podaj adres e-mail.");
                return;
            }

            if (string.IsNullOrWhiteSpace(_password))
            {
                ShowError("Podaj hasło.");
                return;
            }

            if (_password != repeatedPassword)
            {
                ShowError("Hasła nie są takie same.");
                return;
            }

            // tutaj później wywołasz AuthApiClient.RegisterAsync(...)
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Finished?.Invoke(_email, _password);
            Close();
        }

        private void ShowError(string message)
        {
            _messageDialogService.ShowError("Błąd", message, this);
        }
    }
}
