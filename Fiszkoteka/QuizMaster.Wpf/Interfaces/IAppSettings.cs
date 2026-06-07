using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Interfaces
{
    public interface IAppSettings
    {
        string SavedEmail { get; }
        bool RememberLogin { get; }

        void Load();
        void SaveRememberLogin(string email, bool rememberLogin);
    }
}
