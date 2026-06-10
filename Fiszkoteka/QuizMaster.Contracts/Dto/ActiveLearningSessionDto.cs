using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace QuizMaster.Contracts.Dto
{
    [Description("Główny dashboard - Aktywne sesje na głównym interfejsie")]
    public sealed class ActiveLearningSessionDto
    {
        public int Id { get; set; }

        public int FlashcardSetId { get; set; }
        public string FlashcardSetName { get; set; }

        public int TotalFlashcardsCount { get; set; }
        public int ReviewedFlashcardsCount { get; set; }

        public DateTime StartedAt { get; set; }
    }
}
