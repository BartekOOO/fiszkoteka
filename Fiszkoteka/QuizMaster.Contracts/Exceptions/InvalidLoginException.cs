using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class InvalidLoginException : QuizMasterException
    {
        public InvalidLoginException()
            : base("Nieprawidłowy login lub hasło.", 401)
        {

        }

        private InvalidLoginException(string message)
            : base(message, 401)
        {

        }

        public static InvalidLoginException FromMessage(string message)
        {
            return new InvalidLoginException(message);  
        }
    }
}
