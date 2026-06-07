using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class FlashcardSetAccessDeniedException : QuizMasterException
    {
        public FlashcardSetAccessDeniedException()
            : base($"Zestaw fiszek nie należy do użytkownika", 403)
        {

        }

        private FlashcardSetAccessDeniedException(string message)
            : base(message, 403)
        {

        }

        public static FlashcardSetAccessDeniedException FromMessage(string message)
        {
            return new FlashcardSetAccessDeniedException(message);
        }
    }
}
