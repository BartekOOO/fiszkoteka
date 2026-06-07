using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Extensions
{
    public static class ExceptionResponseExtensions
    {
        public static Exception Map(this ExceptionResponse error)
        {
            if (error == null)
                return new Exception("Serwer zwrócił niepoprawną odpowiedź błędu.");

            switch (error.Exception)
            {
                case nameof(InvalidLoginException):
                    return InvalidLoginException.FromMessage(error.Message);

                case nameof(UserAlreadyExistsException):
                    return UserAlreadyExistsException.FromMessage(error.Message);

                case nameof(TokenExpiredException):
                    return TokenExpiredException.FromMessage(error.Message);

                default:
                    return new Exception(error.Message ?? "Wystąpił nieznany błąd.");
            }
        }
    }
}
