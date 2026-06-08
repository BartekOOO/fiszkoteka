using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class LearningSessionFinishedException : QuizMasterException
    {
        public LearningSessionFinishedException()
            : base("Ta sesja nauki została już zakończona.", 400)
        {
        }

        private LearningSessionFinishedException(string message)
            : base(message, 400)
        {
        }

        public static LearningSessionFinishedException FromMessage(string message)
        {
            return new LearningSessionFinishedException(
                string.IsNullOrWhiteSpace(message)
                    ? "Ta sesja nauki została już zakończona."
                    : message);
        }
    }
}
