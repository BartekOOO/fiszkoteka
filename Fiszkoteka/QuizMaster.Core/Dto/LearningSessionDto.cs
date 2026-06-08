using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Dto
{
    public sealed class LearningSessionDto
    {
        public int Id { get; set; }

        public int FlashcardSetId { get; set; }
        public string FlashcardSetName { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }

        public int TotalFlashcardsCount { get; set; }
        public int ReviewedFlashcardsCount { get; set; }

        public int CorrectAnswersCount { get; set; }
        public int WrongAnswersCount { get; set; }

        public bool IsFinished { get; set; }
    }
}
