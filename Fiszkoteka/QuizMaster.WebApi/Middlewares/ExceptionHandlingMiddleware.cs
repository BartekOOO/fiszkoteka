using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Models;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace QuizMaster.WebApi.Middlewares
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (QuizMasterException ex)
            {
                await HandleQuizMasterExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                await HandleUnknownExceptionAsync(context, ex);
            }
        }

        private static async Task HandleQuizMasterExceptionAsync(
            HttpContext context,
            QuizMasterException ex)
        {
            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new ExceptionResponse
            {
                Exception = ex.GetType().Name,
                Message = ex.Message,
                StatusCode = ex.StatusCode
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }

        private static async Task HandleUnknownExceptionAsync(
            HttpContext context,
            Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ExceptionResponse
            {
                Exception = ex.GetType().Name,
                Message = "Wystąpił nieoczekiwany błąd serwera.",
                StatusCode = 500
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
