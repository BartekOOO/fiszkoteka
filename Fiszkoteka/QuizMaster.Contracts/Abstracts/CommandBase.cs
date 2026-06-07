using QuizMaster.Core.Abstracts;
using QuizMaster.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Abstracts
{
    public abstract class CommandBase : QuizMasterObject
    {
        public int UserId { get; set; }
    }
}
