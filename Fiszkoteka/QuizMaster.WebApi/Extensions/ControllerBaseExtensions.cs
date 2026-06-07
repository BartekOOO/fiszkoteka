using Microsoft.AspNetCore.Mvc;
using QuizMaster.Core.Models;
using System.Security.Claims;

namespace QuizMaster.WebApi.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static int GetCurrentUserId(this ControllerBase target)
        {
            var userIdText = target.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userIdText))
                throw new UnauthorizedAccessException("Brak identyfikatora użytkownika w tokenie.");

            return int.Parse(userIdText);
        }
    }
}
