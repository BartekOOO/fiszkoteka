using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Contracts.Auth
{
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
