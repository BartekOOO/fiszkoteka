using QuizMaster.Contracts.Abstracts;
using QuizMaster.Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Commands
{
    public sealed class CreateFlashCardSetCommand : CommandBase
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public int CategoryId { get; set; }

        public CreateFlashCardSetCommand() { }
    }
}
