using QuizMaster.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Dto
{
    public sealed class LearningFlashcardDto
    {
        public int Id { get; set; }
        public int FlashcardSetId { get; set; }

        public string Question { get; set; }
        public string Hint { get; set; }

        public DifficultyLevel Difficulty { get; set; }
    }
}
