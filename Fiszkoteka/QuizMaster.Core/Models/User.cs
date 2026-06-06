using QuizMaster.Core.Abstracts;
using QuizMaster.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Core.Models
{
    public sealed class User : QuizMasterObject
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public User()
        {

        }
    }
}
