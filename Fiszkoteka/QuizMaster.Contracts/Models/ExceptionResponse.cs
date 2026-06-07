using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Models
{
    public sealed class ExceptionResponse
    {
        public string Exception { get; set; }
        public string Message { get; set; } 
        public int StatusCode { get; set; }
    }
}
