using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace QuizMaster.Wpf.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> ReadResponseAsync<T>(
            this HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<ExceptionResponse>(
                    cancellationToken: cancellationToken);

                throw error?.Map() ?? throw new Exception("Nie udało się odczytać wiadomości");
            }

            var result = await response.Content.ReadFromJsonAsync<T>(
                cancellationToken: cancellationToken);

            if (result == null)
                throw new ServerResponseIsEmptyException();

            return result;
        }
    }
}
