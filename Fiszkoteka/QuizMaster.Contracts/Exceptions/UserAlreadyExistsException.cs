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
    }
}
