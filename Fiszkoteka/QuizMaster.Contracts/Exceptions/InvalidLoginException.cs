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
    }
}
