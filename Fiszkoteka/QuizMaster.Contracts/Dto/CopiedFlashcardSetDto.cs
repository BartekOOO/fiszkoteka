using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Dto
{
    public sealed class CopiedFlashcardSetDto
    {
        public string Response {  get; set; }
        public int Id { get; set; }
        public string FromUser { get; set; }
    }
}
