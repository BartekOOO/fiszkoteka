using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public abstract class QuizMasterException : Exception
    {
        public int StatusCode { get; }

        protected QuizMasterException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
