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
            var dialogOwner = ResolveOwner(owner, dialog);

            if (dialogOwner != null)
            {
                try
                {
                    dialog.Owner = dialogOwner;
                }
                catch (InvalidOperationException)
                {

                }
            }

            dialog.ShowDialog();

            return dialog.Result;
        }

        private static Window ResolveOwner(Window owner, Window dialog)
        {
            if (CanUseAsOwner(owner, dialog))
            {
                return owner;
            }

            var mainWindow = Application.Current.MainWindow;

            if (CanUseAsOwner(mainWindow, dialog))
            {
                return mainWindow;
            }

            return null;
        }

        private static bool CanUseAsOwner(Window candidate, Window dialog)
        {
            if (candidate == null || ReferenceEquals(candidate, dialog))
            {
                return false;
            }

            if (!candidate.IsLoaded)
            {
                return false;
            }

            return PresentationSource.FromVisual(candidate) != null;
        }
    }
}
