using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Auth
{
    public sealed class LogoutRequest
    {
        public string Token { get; set; }
    }
}
