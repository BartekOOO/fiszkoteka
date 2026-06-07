using QuizMaster.Contracts.Auth;
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
        private readonly HttpClient _httpClient;

        public LoginWindow()
        {
            InitializeComponent();

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7001")
            };
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var request = new LoginRequest
                {
                    Email = EmailTextBox.Text,
                    Password = PasswordBox.Password
                };

                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Nieprawidłowy email lub hasło.");
                    return;
                }

                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (authResponse == null)
                {
                    MessageBox.Show("Nie udało się odczytać odpowiedzi z serwera.");
                    return;
                }

                AppSession.UserId = authResponse.UserId;
                AppSession.UserName = authResponse.UserName;
                AppSession.Email = authResponse.Email;
                AppSession.Token = authResponse.Token;

                var mainWindow = new MainWindow();
                mainWindow.Show();

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd logowania: " + ex.Message);
            }
        }
    }
}
