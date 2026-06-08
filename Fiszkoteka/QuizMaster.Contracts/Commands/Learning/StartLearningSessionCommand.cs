using QuizMaster.Contracts.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Commands.Learning
{
    public sealed class StartLearningSessionCommand : CommandBase
    {
        public int FlashcardSetId { get; set; }

        public StartLearningSessionCommand()
        {
        }
    }
}
