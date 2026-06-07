using QuizMaster.Wpf.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace QuizMaster.Wpf.Interfaces
{
    public interface IMessageDialogService
    {
        MessageDialogResult ShowInfo(string title, string message, Window owner = null);
        MessageDialogResult ShowSuccess(string title, string message, Window owner = null);
        MessageDialogResult ShowWarning(string title, string message, Window owner = null);
        MessageDialogResult ShowError(string title, string message, Window owner = null);

        MessageDialogResult ShowQuestion(string title, string message, Window owner = null);
    }
}
