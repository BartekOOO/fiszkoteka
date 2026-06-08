using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class LearningSessionNotFoundException : QuizMasterException
    {
        public LearningSessionNotFoundException(int id)
            : base($"Nie znaleziono sesji nauki o identyfikatorze {id}.", 404)
        {
        }

        private LearningSessionNotFoundException(string message)
            : base(message, 404)
        {
        }

        public static LearningSessionNotFoundException FromMessage(string message)
        {
            return new LearningSessionNotFoundException(
                string.IsNullOrWhiteSpace(message)
                    ? "Nie znaleziono sesji nauki."
                    : message);
        }
    }
}
