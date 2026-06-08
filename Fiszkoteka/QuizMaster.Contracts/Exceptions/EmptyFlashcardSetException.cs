using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class EmptyFlashcardSetException : QuizMasterException
    {
        public EmptyFlashcardSetException(int flashcardSetId)
            : base($"Zestaw fiszek o identyfikatorze {flashcardSetId} nie zawiera żadnych fiszek.", 400)
        {
        }

        private EmptyFlashcardSetException(string message)
            : base(message, 400)
        {
        }

        public static EmptyFlashcardSetException FromMessage(string message)
        {
            return new EmptyFlashcardSetException(
                string.IsNullOrWhiteSpace(message)
                    ? "Zestaw fiszek nie zawiera żadnych fiszek."
                    : message);
        }
    }
}
