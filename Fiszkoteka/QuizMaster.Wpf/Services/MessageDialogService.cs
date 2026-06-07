using QuizMaster.Wpf.Dialogs;
using QuizMaster.Wpf.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace QuizMaster.Wpf.Services
{
    public sealed class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowInfo(string title, string message, Window owner = null)
        {
            return Show(title, message, MessageDialogType.Info, owner);
        }

        public MessageDialogResult ShowSuccess(string title, string message, Window owner = null)
        {
            return Show(title, message, MessageDialogType.Success, owner);
        }

        public MessageDialogResult ShowWarning(string title, string message, Window owner = null)
        {
            return Show(title, message, MessageDialogType.Warning, owner);
        }

        public MessageDialogResult ShowError(string title, string message, Window owner = null)
        {
            return Show(title, message, MessageDialogType.Error, owner);
        }

        public MessageDialogResult ShowQuestion(string title, string message, Window owner = null)
        {
            return Show(title, message, MessageDialogType.Question, owner);
        }

        private MessageDialogResult Show(
            string title,
            string message,
            MessageDialogType type,
            Window owner)
        {
            var dialog = new MessageDialogWindow(title, message, type);

            if (owner != null)
            {
                dialog.Owner = owner;
            }
            else if (Application.Current.MainWindow != null && Application.Current.MainWindow != dialog)
            {
                dialog.Owner = Application.Current.MainWindow;
            }

            dialog.ShowDialog();

            return dialog.Result;
        }
    }
}
