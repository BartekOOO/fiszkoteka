using QuizMaster.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Models
{
    public class UserFlashcardProgress : QuizMasterObject
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int FlashcardId { get; set; }

        public int CorrectAnswersCount { get; set; }
        public int WrongAnswersCount { get; set; }

        public int MasteryLevel { get; set; }

        public DateTime? LastReviewedAt { get; set; }
        public DateTime? NextReviewAt { get; set; }

        public UserFlashcardProgress()
        {

        }
    }
}
