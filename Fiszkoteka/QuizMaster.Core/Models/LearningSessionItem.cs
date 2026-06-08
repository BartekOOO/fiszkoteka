using QuizMaster.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Models
{
    public sealed class LearningSessionItem : QuizMasterObject
    {
        public int Id { get; set; }

        public int LearningSessionId { get; set; }
        public LearningSession LearningSession { get; set; }

        public int FlashcardId { get; set; }
        public Flashcard Flashcard { get; set; }

        public bool IsAnswered { get; set; }
        public bool? IsCorrect { get; set; }

        public DateTime? AnsweredAt { get; set; }
    }
}
