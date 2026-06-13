using QuizMaster.Contracts.Abstracts;
using QuizMaster.Core.Enums;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Commands.Flashcards
{
    public sealed class CreateFlashcardCommand : CommandBase
    {
        public int FlashcardSetId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string? Hint { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        public CreateFlashcardCommand()
        {

        }

        public CreateFlashcardCommand(Flashcard flashcard)
        {
            if (flashcard == null)
                throw new ArgumentNullException(nameof(flashcard));

            this.Question = flashcard.Question;
            this.Answer = flashcard.Answer;
            this.Hint = flashcard.Hint;
            this.Difficulty = flashcard.Difficulty;
            this.FlashcardSetId = flashcard.FlashcardSetId;
        }
    }
}
