using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QuizMaster.Contracts.Dto
{
    [Description("Główny dashboard - Ostatnie zestawy")]
    public sealed class RecentFlashcardSetDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FlashcardsCount { get; set; }

        public DateTime? LastLearningAt { get; set; }

        public string LastLearningText { get; set; }
    }
}
