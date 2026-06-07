using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class FlashcardNotFoundException : QuizMasterException
    {
        public FlashcardNotFoundException(int id)
            : base($"Nie znaleziono fiszki o identyfikatorze {id}.", 404)
        {
        }

        private FlashcardNotFoundException(string message)
            : base(message, 404)
        {
        }

        public static FlashcardNotFoundException FromMessage(string message)
        {
            return new FlashcardNotFoundException(
                string.IsNullOrWhiteSpace(message)
                    ? "Nie znaleziono fiszki."
                    : message);
        }
    }
}
