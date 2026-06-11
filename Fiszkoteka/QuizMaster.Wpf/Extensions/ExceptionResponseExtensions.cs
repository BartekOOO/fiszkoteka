using QuizMaster.Contracts.Exceptions;
using QuizMaster.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Extensions
{
    public static class ExceptionResponseExtensions
    {
        public static void Map(this ExceptionResponse error)
        {
            if (error == null)
                throw new Exception("Serwer zwrócił niepoprawną odpowiedź błędu.");

            switch (error.Exception)
            {
                case nameof(InvalidLoginException):
                    throw InvalidLoginException.FromMessage(error.Message);

                case nameof(UserAlreadyExistsException):
                    throw UserAlreadyExistsException.FromMessage(error.Message);

                case nameof(TokenExpiredException):
                    throw TokenExpiredException.FromMessage(error.Message);

                case nameof(ServerResponseIsEmptyException):
                    throw ServerResponseIsEmptyException.FromMessage(error.Message);

                case nameof(CategoryNotFoundException):
                    throw CategoryNotFoundException.FromMessage(error.Message);

                case nameof(EmptyFieldException):
                    throw EmptyFieldException.FromMessage(error.Message);

                case nameof(EmptyFlashcardSetException):
                    throw EmptyFlashcardSetException.FromMessage(error.Message);

                case nameof(FlashcardNotFoundException):
                    throw FlashcardNotFoundException.FromMessage(error.Message);

                case nameof(FlashcardAccessDeniedException):
                    throw FlashcardAccessDeniedException.FromMessage(error.Message);

                case nameof(FlashcardSetNotFoundException):
                    throw FlashcardSetNotFoundException.FromMessage(error.Message);

                case nameof(FlashcardSetAccessDeniedException):
                    throw FlashcardSetAccessDeniedException.FromMessage(error.Message);

                case nameof(LearningSessionNotFoundException):
                    throw LearningSessionNotFoundException.FromMessage(error.Message);

                case nameof(LearningSessionFinishedException):
                    throw LearningSessionFinishedException.FromMessage(error.Message);

                case nameof(LearningSessionExhaustedException):
                    throw LearningSessionExhaustedException.FromMessage(error.Message);

                case nameof(ActiveLearningSessionExistsException):
                    throw ActiveLearningSessionExistsException.FromMessage(error.Message);

                case nameof(UserNotExistsException):
                    throw UserNotExistsException.FromMessage(error.Message);

                default:
                    throw new Exception(error.Message ?? "Wystąpił nieznany błąd.");
            }
        }
    }
}
