using QuizMaster.Core.Abstracts;
using QuizMaster.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Models
{
    public sealed class Flashcard : QuizMasterObject
    {
        public int Id { get; set; }

        public int FlashcardSetId { get; set; }
        public FlashcardSet FlashcardSet { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }
        public string Hint { get; set; }

        public DifficultyLevel DifficultyLevel { get; set; }

        public List<UserFlashcardProgress> Progresses { get; set; }

        public Flashcard()
        {
            this.Progresses = new List<UserFlashcardProgress>();
        }
    }
}
