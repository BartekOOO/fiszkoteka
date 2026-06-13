using QuizMaster.Contracts.Commands.Flashcards;
using QuizMaster.Core.Enums;
using QuizMaster.Wpf.Delegates;
using QuizMaster.Wpf.Enums;
using QuizMaster.Wpf.Interfaces;
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
        private readonly IMessageDialogService _messageDialogService;

        private WindowContext _context;
        public WindowContext Context
        {
            get
            {
                if (_flashcardSetId is null && _flashcardId is null)
                    throw new Exception("Nie zainicjalizowano okna");
                return _context;
            }
        }

        private int? _flashcardSetId;
        private int? _flashcardId;

        public CreateFlashcardHandler OnCreatedFlashcard;
        public EditFlashcardHandler OnEditedFlashcard;


        public EditFlashcardWindow(IMessageDialogService messageDialogService)
        {
            InitializeComponent();
            _messageDialogService = messageDialogService;
        }

        public void InitializeData(
            string question,
            string answer,
            string hint,
            DifficultyLevel difficulty,
            int? flashcardSetId,
            int? flashcardId)
        {
            _flashcardSetId = flashcardSetId;
            _flashcardId = flashcardId;

            if (_flashcardId is null && _flashcardSetId is null)
                throw new Exception("Nie można określić kontekstu okna");

            _context = WindowContext.Adding;
            if (_flashcardId is not null)
                _context = WindowContext.Editing;
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

            if(Context == WindowContext.Adding)
            {
                var command = new CreateFlashcardCommand
                {
                    FlashcardSetId = _flashcardSetId!.Value,
                    Question = QuestionTextBox.Text,
                    Answer = AnswerTextBox.Text,
                    Hint = HintTextBox.Text,
                    Difficulty = DifficultyLevel.Easy //Do zaimplemntowania
                };

                if(OnCreatedFlashcard is null)
                {
                    _messageDialogService.ShowError("Błąd",
                        "Nie podpięto żadnego zdarzenia", this);
                    return;
                }

                var result = OnCreatedFlashcard(this, command, _flashcardSetId!.Value);

                if (result)
                    Close();
            }
            else
            {
                var command = new UpdateFlashcardCommand
                {
                    Question = QuestionTextBox.Text,
                    Answer = AnswerTextBox.Text,
                    Hint = HintTextBox.Text,
                    Difficulty = DifficultyLevel.Easy //Do zaimplemntowania
                };

                if (OnEditedFlashcard is null)
                {
                    _messageDialogService.ShowError("Błąd",
                        "Nie podpięto żadnego zdarzenia", this);
                    return;
                }

                var result = OnEditedFlashcard(this, command, _flashcardId!.Value);

                if (result)
                    Close();
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
