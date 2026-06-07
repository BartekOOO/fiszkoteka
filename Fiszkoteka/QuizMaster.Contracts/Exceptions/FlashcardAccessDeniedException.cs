using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class FlashcardAccessDeniedException : QuizMasterException
    {
        public FlashcardAccessDeniedException()
            : base($"Fiszka nie należy do użytkownika", 403)
        {

        }

        private FlashcardAccessDeniedException(string message)
            : base(message, 403)
        {

        }

        public static FlashcardAccessDeniedException FromMessage(string message)
        {
            return new FlashcardAccessDeniedException(message);
        }
    }
}
