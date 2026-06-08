using QuizMaster.Contracts.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Commands.Learning
{
    public sealed class AnswerFlashcardCommand : CommandBase
    {
        public int FlashcardId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
