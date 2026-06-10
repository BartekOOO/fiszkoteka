using QuizMaster.Wpf.Delegates;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Interfaces
{
    public interface ISessionEvents
    {
        event SessionExpiredHandler OnSessionExpired;
    }
}
