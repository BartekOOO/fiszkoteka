using QuizMaster.Contracts.Abstracts;
using QuizMaster.Core.Abstracts;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace QuizMaster.Contracts.Commands.FlashcardSets
{
    public sealed class CreateFlashcardSetCommand : CommandBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        public CreateFlashcardSetCommand() { }
        public CreateFlashcardSetCommand(FlashcardSet flashcardSet)
        {
            if (flashcardSet == null)
                throw new ArgumentNullException(nameof(flashcardSet));

            this.Name = flashcardSet.Name;  
            this.Description = flashcardSet.Description;
            this.CategoryId = flashcardSet.CategoryId;
        }

        public FlashcardSet ToFlashcardSet()
        {
            return new FlashcardSet()
            {
                Name = this.Name,
                Description = this.Description,
                CategoryId = this.CategoryId,
            };
        }
    }
}
