using QuizMaster.Contracts.Auth;
using QuizMaster.Wpf.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace QuizMaster.Wpf.Services
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Nieprawidłowy email lub hasło.");
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (result == null)
            {
                throw new Exception("Serwer zwrócił pustą odpowiedź.");
            }

            return result;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", request);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Nie udało się zarejestrować użytkownika.");
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (result == null)
            {
                throw new Exception("Serwer zwrócił pustą odpowiedź.");
            }

            return result;
        }
    }
}
