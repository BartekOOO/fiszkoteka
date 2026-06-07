using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Interfaces
{
    public interface IAppSession
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

        public bool IsLoggedIn { get; }
        void Clear();
    }
}
