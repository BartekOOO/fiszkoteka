using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuizMaster.Wpf.Dialogs
{
    /// <summary>
    /// Logika interakcji dla klasy MessageDialogWindow.xaml
    /// </summary>
    public partial class MessageDialogWindow : Window
    {
        public MessageDialogResult Result { get; private set; } = MessageDialogResult.None;

        public MessageDialogWindow(
            string title,
            string message,
            MessageDialogType type)
        {
            InitializeComponent();

            TitleTextBlock.Text = title;
            MessageTextBlock.Text = message;

            ConfigureType(type);
            ConfigureButtons(type);
        }

        private void ConfigureType(MessageDialogType type)
        {
            switch (type)
            {
                case MessageDialogType.Info:
                    HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(37, 99, 235));
                    IconTextBlock.Text = "ℹ";
                    break;

                case MessageDialogType.Success:
                    HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(22, 163, 74));
                    IconTextBlock.Text = "✓";
                    break;

                case MessageDialogType.Warning:
                    HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(234, 179, 8));
                    IconTextBlock.Text = "!";
                    break;

                case MessageDialogType.Error:
                    HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(220, 38, 38));
                    IconTextBlock.Text = "×";
                    break;

                case MessageDialogType.Question:
                    HeaderBorder.Background = new SolidColorBrush(Color.FromRgb(79, 70, 229));
                    IconTextBlock.Text = "?";
                    break;
            }
        }

        private void ConfigureButtons(MessageDialogType type)
        {
            if (type == MessageDialogType.Question)
            {
                OkButtonPanel.Visibility = Visibility.Collapsed;
                QuestionButtonPanel.Visibility = Visibility.Visible;
            }
            else
            {
                OkButtonPanel.Visibility = Visibility.Visible;
                QuestionButtonPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageDialogResult.Ok;
            DialogResult = true;
            Close();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageDialogResult.Yes;
            DialogResult = true;
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageDialogResult.No;
            DialogResult = false;
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageDialogResult.Cancel;
            DialogResult = false;
            Close();
        }
    }
}
