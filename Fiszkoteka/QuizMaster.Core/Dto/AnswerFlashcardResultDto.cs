using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Dto
{
    public sealed class AnswerFlashcardResultDto
    {
        public int FlashcardId { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int WrongAnswersCount { get; set; }

        public bool SessionFinished { get; set; }
        public int ReviewedFlashcardsCount { get; set; }
    }
}
