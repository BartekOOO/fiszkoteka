using QuizMaster.Contracts.Auth;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Models;
using QuizMaster.Wpf.Extensions;
using QuizMaster.Wpf.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace QuizMaster.Wpf.Services
{
    public sealed class ApiClient : IApiClient
    {
        private readonly SessionEvents _sessionEvents;
        private readonly HttpClient _httpClient;
        private readonly IAppSession _appSession;

        public ApiClient(SessionEvents sessionEvents, HttpClient httpClient, IAppSession appSession)
        {
            _sessionEvents = sessionEvents;
            _httpClient = httpClient;
            _appSession = appSession;
        }

        public async Task<TResponse> GetAsync<TResponse>(
            string path,
            CancellationToken cancellationToken = default)
        {
            SetAuthorizationHeader();

            var response = await _httpClient.GetAsync(path, cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);

            return await ReadAsync<TResponse>(response, cancellationToken);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default)
        {
            SetAuthorizationHeader();

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
            SetAuthorizationHeader();

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
            SetAuthorizationHeader();

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
            SetAuthorizationHeader();

            var response = await _httpClient.PutAsJsonAsync(
                path,
                payload,
                cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);

            return await ReadAsync<TResponse>(response, cancellationToken);
        }

        public async Task<TResponse> PutAsync<TResponse>(
            string path,
            CancellationToken cancellationToken = default)
        {
            SetAuthorizationHeader();

            var response = await _httpClient.PutAsync(
                path,
                null,
                cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);

            return await ReadAsync<TResponse>(response, cancellationToken);
        }

        public async Task PutAsync<TRequest>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default)
        {
            SetAuthorizationHeader();

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
            SetAuthorizationHeader();

            var response = await _httpClient.DeleteAsync(path, cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);
        }

        private async Task EnsureSuccessAsync(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            if (response.IsSuccessStatusCode)
                return;

            var error = await response.Content.
                    ReadFromJsonAsync<ExceptionResponse>(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized ||
                error.Exception == nameof(TokenExpiredException))
            {
                _sessionEvents.InvokeSessionExpired();
            }

            if (error != null)
                error.Map();    
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

        private void SetAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (string.IsNullOrWhiteSpace(_appSession.Token))
                return;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _appSession.Token);
        }

        public async Task<TResponse> PostAsync<TResponse>(string path, CancellationToken cancellationToken = default)
        {
            SetAuthorizationHeader();

            var response = await _httpClient.PostAsync(
                path,
                null,
                cancellationToken);

            await EnsureSuccessAsync(response, cancellationToken);

            return await ReadAsync<TResponse>(response, cancellationToken);
        }
    }
}
