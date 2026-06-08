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

                case nameof(ServerResponseIsEmptyException):
                    return ServerResponseIsEmptyException.FromMessage(error.Message);

                case nameof(CategoryNotFoundException):
                    return CategoryNotFoundException.FromMessage(error.Message);

                case nameof(EmptyFieldException):
                    return EmptyFieldException.FromMessage(error.Message);

                case nameof(EmptyFlashcardSetException):
                    return EmptyFlashcardSetException.FromMessage(error.Message);

                case nameof(FlashcardNotFoundException):
                    return FlashcardNotFoundException.FromMessage(error.Message);

                case nameof(FlashcardAccessDeniedException):
                    return FlashcardAccessDeniedException.FromMessage(error.Message);

                case nameof(FlashcardSetNotFoundException):
                    return FlashcardSetNotFoundException.FromMessage(error.Message);

                case nameof(FlashcardSetAccessDeniedException):
                    return FlashcardSetAccessDeniedException.FromMessage(error.Message);

                case nameof(LearningSessionNotFoundException):
                    return LearningSessionNotFoundException.FromMessage(error.Message);

                case nameof(LearningSessionFinishedException):
                    return LearningSessionFinishedException.FromMessage(error.Message);

                case nameof(LearningSessionExhaustedException):
                    return LearningSessionExhaustedException.FromMessage(error.Message);

                default:
                    return new Exception(error.Message ?? "Wystąpił nieznany błąd.");
            }
        }
    }
}
