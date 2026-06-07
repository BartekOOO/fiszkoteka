using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class UserAlreadyExistsException : QuizMasterException
    {
        public UserAlreadyExistsException(string login)
            : base($"Użytkownik '{login}' już istnieje.", 409)
        {

        }

        private UserAlreadyExistsException(string message, bool useRaw)
            : base(message, 409)
        {

        }

        public static UserAlreadyExistsException FromMessage(string message)
        {
            return new UserAlreadyExistsException(message, true);
        }
    }
}
