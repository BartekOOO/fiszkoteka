using QuizMaster.Contracts.Abstracts;
using QuizMaster.Core.Abstracts;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Commands.FlashcardSets
{
    public sealed class UpdateFlashcardSetCommand : CommandBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsPublic { get; set; }
        public int? CategoryId { get; set; }

        public UpdateFlashcardSetCommand() { }
        public UpdateFlashcardSetCommand(FlashcardSet flashcardSet)
        {
            if(flashcardSet == null)
                throw new ArgumentNullException(nameof(flashcardSet));

            this.Name = flashcardSet.Name;
            this.Description = flashcardSet.Description;
            this.IsPublic = flashcardSet.IsPublic;
            this.CategoryId = flashcardSet.CategoryId;
        }
    }
}
