using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class LearningSessionExhaustedException : QuizMasterException
    {
        public LearningSessionExhaustedException()
            : base("Sesja nauki została wyczerpana. Nie ma kolejnych fiszek do wyświetlenia.", 400)
        {
        }

        private LearningSessionExhaustedException(string message)
            : base(message, 400)
        {
        }

        public static LearningSessionExhaustedException FromMessage(string message)
        {
            return new LearningSessionExhaustedException(
                string.IsNullOrWhiteSpace(message)
                    ? "Sesja nauki została wyczerpana."
                    : message);
        }
    }
}
