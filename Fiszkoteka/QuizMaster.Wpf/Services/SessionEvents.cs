using QuizMaster.Wpf.Delegates;
using QuizMaster.Wpf.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuizMaster.Wpf.Services
{
    public sealed class SessionEvents : ISessionEvents
    {
        public event SessionExpiredHandler OnSessionExpired;

        public void InvokeSessionExpired()
        {
            OnSessionExpired?.Invoke();
        }
    }
}
