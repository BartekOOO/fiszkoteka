using QuizMaster.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Models
{
    public class Category : QuizMasterObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<FlashcardSet> FlashcardSets { get; set; }

        public Category()
        {
            FlashcardSets = new List<FlashcardSet>();
        }
    }
}
