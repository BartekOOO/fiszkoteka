using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Dto
{
    public sealed class FlashcardSetListItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int FlashcardsCount { get; set; }

        public bool IsPublic { get; set; }
        public string Author { get; set; }

        public DateTime CreatedAt { get; set; }

        public FlashcardSetListItemDto()
        {

        }
    }
}
