using QuizMaster.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class TooManyActiveLearningSessionsException : QuizMasterException
    {
        public TooManyActiveLearningSessionsException()
            : base($"Posiadasz już zbyt wiele rozpoczętych sesji.", 403)
        {

        }

        private TooManyActiveLearningSessionsException(string message)
            : base(message, 403)
        {

        }

        public static TooManyActiveLearningSessionsException FromMessage(string message)
        {
            return new TooManyActiveLearningSessionsException(message);
        }
    }
}
