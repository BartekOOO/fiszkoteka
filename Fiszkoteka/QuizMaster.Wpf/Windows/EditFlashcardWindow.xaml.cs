using QuizMaster.Core.Enums;
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

namespace QuizMaster.Wpf.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy EditFlashcardWindow.xaml
    /// </summary>
    public partial class EditFlashcardWindow : Window
    {
        public string Question { get; private set; }
        public string Answer { get; private set; }
        public string Hint { get; private set; }
        public DifficultyLevel Difficulty { get; private set; }

        public EditFlashcardWindow()
        {
            InitializeComponent();

            Difficulty = DifficultyLevel.Easy;
        }

        public EditFlashcardWindow(
            string question,
            string answer,
            string hint,
            DifficultyLevel difficulty)
            : this()
        {
            QuestionTextBox.Text = question;
            AnswerTextBox.Text = answer;
            HintTextBox.Text = hint;

            Difficulty = difficulty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text))
            {
                MessageBox.Show("Podaj pytanie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(AnswerTextBox.Text))
            {
                MessageBox.Show("Podaj odpowiedź.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Question = QuestionTextBox.Text.Trim();
            Answer = AnswerTextBox.Text.Trim();
            Hint = string.IsNullOrWhiteSpace(HintTextBox.Text)
                ? null
                : HintTextBox.Text.Trim();

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
