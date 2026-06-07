using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Auth
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
