using QuizMaster.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Models
{
    public sealed class LearningSession : QuizMasterObject
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int FlashcardSetId { get; set; }
        public FlashcardSet FlashcardSet { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int TotalFlashcardsCount { get; set; }
        public int ReviewedFlashcardsCount { get; set; }
        public int CorrectAnswersCount { get; set; }
        public int WrongAnswersCount { get; set; }
        public bool IsFinished
        {
            get { return FinishedAt.HasValue; }
        }

        public LearningSession()
        {
            StartedAt = DateTime.UtcNow;
        }
    }
}
