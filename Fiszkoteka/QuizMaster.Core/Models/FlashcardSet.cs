using QuizMaster.Core.Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Models
{
    public sealed class FlashcardSet : QuizMasterObject, IEnumerable<Flashcard>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int OwnerId { get; set; }
        public bool IsPublic { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Flashcard> Flashcards { get; set; }
        public FlashcardSet()
        {
            this.Flashcards = new List<Flashcard>();
        }

        public IEnumerator<Flashcard> GetEnumerator()
        {
            return this.Flashcards.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
