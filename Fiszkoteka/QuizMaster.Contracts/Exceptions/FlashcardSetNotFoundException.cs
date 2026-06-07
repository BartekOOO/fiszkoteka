using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class FlashcardSetNotFoundException : QuizMasterException
    {
        public FlashcardSetNotFoundException(int id)
            : base($"Nie znaleziono zestawu fiszek o identyfikatorze {id}.", 404)
        {
        }

        private FlashcardSetNotFoundException(string message)
            : base(message, 404)
        {
        }

        public static FlashcardSetNotFoundException FromMessage(string message)
        {
            return new FlashcardSetNotFoundException(
                string.IsNullOrWhiteSpace(message)
                    ? "Nie znaleziono zestawu fiszek."
                    : message);
        }
    }
}
