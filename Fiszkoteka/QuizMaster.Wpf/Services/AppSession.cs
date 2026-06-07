using QuizMaster.Wpf.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Services
{
    public sealed class AppSession : IAppSession
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);
    }
}
