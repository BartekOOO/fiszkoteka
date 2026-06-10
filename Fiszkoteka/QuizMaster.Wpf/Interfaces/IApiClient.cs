using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> GetAsync<TResponse>(
            string path,
            CancellationToken cancellationToken = default);

        Task<TResponse> PostAsync<TRequest, TResponse>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default);

        Task PostAsync<TRequest>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default);

        Task PostAsync(
            string path,
            CancellationToken cancellationToken = default);

        Task<TResponse> PutAsync<TRequest, TResponse>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default);

        Task PutAsync<TRequest>(
            string path,
            TRequest payload,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            string path,
            CancellationToken cancellationToken = default);
    }

}
