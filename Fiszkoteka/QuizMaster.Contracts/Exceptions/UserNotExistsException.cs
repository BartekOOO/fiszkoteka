using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class UserNotExistsException : QuizMasterException
    {
        public UserNotExistsException(string login)
            : base($"Użytkownik o nazwie '{login}' nie istnieje.", 404)
        {

        }

        private UserNotExistsException(string message, bool useRaw)
            : base(message, 404)
        {

        }

        public static UserNotExistsException FromMessage(string message)
        {
            return new UserNotExistsException(message, true);
        }
    }
}
