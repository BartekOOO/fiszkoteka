using QuizMaster.Contracts.Auth;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Models;
using QuizMaster.Wpf.Extensions;
using QuizMaster.Wpf.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace QuizMaster.Wpf.Services
{
    public sealed class ApiClient : IApiClient
    {
        private readonly SessionEvents _sessionEvents;
        private readonly HttpClient _httpClient;

        public ApiClient(SessionEvents sessionEvents, HttpClient httpClient)
        {
            _sessionEvents = sessionEvents;
            _httpClient = httpClient;
        }

        public async Task<TResponse> GetAsync<TResponse>(
            string path,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync(path, cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);

            return await ReadAsync<TResponse>(response, cancellationToken);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(
                path,
                payload,
                cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);

            return await ReadAsync<TResponse>(response, cancellationToken);
        }

        public async Task PostAsync<TRequest>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsJsonAsync(
                path,
                payload,
                cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);
        }

        public async Task PostAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PostAsync(
                path,
                null,
                cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PutAsJsonAsync(
                path,
                payload,
                cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);

            return await ReadAsync<TResponse>(response, cancellationToken);
        }

        public async Task PutAsync<TRequest>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.PutAsJsonAsync(
                path,
                payload,
                cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);
        }

        public async Task DeleteAsync(
            string path,
            CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.DeleteAsync(path, cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);
        }

        private async Task EnsureSuccessAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            if (response.IsSuccessStatusCode)
                return;

            var exception = await CreateExceptionAsync(response, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                exception is TokenExpiredException)
            {
                _sessionEvents.InvokeSessionExpired();
            }

            throw exception;
        }

        private static async Task<Exception> CreateExceptionAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            try
            {
                var error = await response.Content.
                    ReadFromJsonAsync<ExceptionResponse>(cancellationToken);

                if (error != null)
                    return error.Map();
            }
            catch
            {

            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                return TokenExpiredException.FromMessage("Sesja wygasła. Zaloguj się ponownie.");

            return new Exception($"Serwer zwrócił błąd HTTP {(int)response.StatusCode}.");
        }

        private static async Task<TResponse> ReadAsync<TResponse>(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            var result = await response.Content.ReadFromJsonAsync<TResponse>(
                cancellationToken);

            if (result == null)
                throw new ServerResponseIsEmptyException();

            return result;
        }
    }
}
