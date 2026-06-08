using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class ActiveLearningSessionExistsException : QuizMasterException
    {
        public ActiveLearningSessionExistsException()
            : base("Nie można usunąć elementu, ponieważ istnieje aktywna sesja nauki.", 409)
        {
        }

        private ActiveLearningSessionExistsException(string message)
            : base(message, 409)
        {
        }

        public static ActiveLearningSessionExistsException FromMessage(string message)
        {
            return new ActiveLearningSessionExistsException(
                string.IsNullOrWhiteSpace(message)
                    ? "Nie można usunąć elementu, ponieważ istnieje aktywna sesja nauki."
                    : message);
        }
    }
}
