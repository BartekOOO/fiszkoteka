using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Exceptions
{
    public sealed class ServerResponseIsEmptyException : QuizMasterException
    {
        public ServerResponseIsEmptyException()
            : base("Serwer zwrócił pustą odpowiedź.", 500)
        {

        }

        private ServerResponseIsEmptyException(string message)
            : base(message, 500)
        {

        }

        public static ServerResponseIsEmptyException FromMessage(string message)
        {
            return new ServerResponseIsEmptyException(message);
        }
    }
}
