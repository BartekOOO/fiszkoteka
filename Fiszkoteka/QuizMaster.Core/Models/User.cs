using QuizMaster.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Models
{
    public class User : QuizMasterObject
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<FlashcardSet> FlashcardSets { get; set; }
        public ICollection<UserFlashcardProgress> FlashcardProgresses { get; set; }

        public User()
        {
            this.FlashcardProgresses = new List<UserFlashcardProgress>();   
            this.FlashcardSets = new List<FlashcardSet>();
        }
    }
}
