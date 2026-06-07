using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class TokenExpiredException : QuizMasterException
    {
        public TokenExpiredException()
            : base("Token autoryzacyjny wygasł. Zaloguj się ponownie.", 401)
        {

        }

        private TokenExpiredException(string message)
            : base(message, 401)
        {

        }

        public static TokenExpiredException FromMessage(string message)
        {
            return new TokenExpiredException(message);
        }
    }
}
