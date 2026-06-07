using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf
{
    public static class AppSession
    {
        public static int UserId { get; set; }
        public static string UserName { get; set; }
        public static string Email { get; set; }
        public static string Token { get; set; }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);
    }
}
