using QuizMaster.Core.Enums;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.ViewModels
{
    public sealed class FlashcardListItemViewModel
    {
        public int Id { get; }

        public string Question { get; set; }
        public string Answer { get; set; }
        public string Hint { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        public string HintText
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Hint))
                    return "Brak podpowiedzi";

                return $"Podpowiedź: {Hint}";
            }
        }

        public FlashcardListItemViewModel(Flashcard flashcard)
        {
            Id = flashcard.Id;
            Question = flashcard.Question;
            Answer = flashcard.Answer;
            Hint = flashcard.Hint;
            Difficulty = flashcard.Difficulty;
        }
    }
}
