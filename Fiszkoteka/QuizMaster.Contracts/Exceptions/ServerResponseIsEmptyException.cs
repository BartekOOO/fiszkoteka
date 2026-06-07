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
    }
}
