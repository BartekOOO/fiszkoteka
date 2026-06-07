using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class EmptyFieldException : QuizMasterException
    {
        public EmptyFieldException(string filedName)
            : base($"Pole {filedName} nie może być puste.", 403)
        {
        }

        private EmptyFieldException(string message, bool rawText)
            : base(message, 403)
        {

        }

        public static EmptyFieldException FromMessage(string message)
        {
            return new EmptyFieldException(message, true);
        }
    }
}
