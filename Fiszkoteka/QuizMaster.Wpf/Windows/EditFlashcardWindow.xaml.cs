using QuizMaster.Contracts.Commands.Flashcards;
using QuizMaster.Core.Enums;
using QuizMaster.Wpf.Delegates;
using QuizMaster.Wpf.Enums;
using QuizMaster.Wpf.Interfaces;
using System;
using System.Linq;
using System.Windows;

namespace QuizMaster.Wpf.Windows
{
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

            LoadDifficultyLevels();
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

            QuestionTextBox.Text = question ?? string.Empty;
            AnswerTextBox.Text = answer ?? string.Empty;
            HintTextBox.Text = hint ?? string.Empty;

            DifficultyComboBox.SelectedValue = difficulty;
        }

        private void LoadDifficultyLevels()
        {
            var items = Enum.GetValues(typeof(DifficultyLevel))
                .Cast<DifficultyLevel>()
                .Select(x => new DifficultyLevelItem
                {
                    Value = x,
                    Name = GetDifficultyName(x)
                })
                .ToList();

            DifficultyComboBox.ItemsSource = items;
            DifficultyComboBox.DisplayMemberPath = nameof(DifficultyLevelItem.Name);
            DifficultyComboBox.SelectedValuePath = nameof(DifficultyLevelItem.Value);

            DifficultyComboBox.SelectedValue = DifficultyLevel.Easy;
        }

        private static string GetDifficultyName(DifficultyLevel difficulty)
        {
            switch (difficulty)
            {
                case DifficultyLevel.Easy:
                    return "Łatwy";

                case DifficultyLevel.Medium:
                    return "Średni";

                case DifficultyLevel.Hard:
                    return "Trudny";

                default:
                    return difficulty.ToString();
            }
        }

        private DifficultyLevel GetSelectedDifficulty()
        {
            if (DifficultyComboBox.SelectedValue is DifficultyLevel difficulty)
                return difficulty;

            return DifficultyLevel.Easy;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var question = QuestionTextBox.Text?.Trim();
            var answer = AnswerTextBox.Text?.Trim();
            var hint = HintTextBox.Text?.Trim();
            var difficulty = GetSelectedDifficulty();

            if (string.IsNullOrWhiteSpace(question))
            {
                _messageDialogService.ShowWarning(
                    "Błąd",
                    "Podaj pytanie.",
                    this);

                return;
            }

            if (string.IsNullOrWhiteSpace(answer))
            {
                _messageDialogService.ShowWarning(
                    "Błąd",
                    "Podaj odpowiedź.",
                    this);

                return;
            }

            if (string.IsNullOrWhiteSpace(hint))
                hint = null;

            if (Context == WindowContext.Adding)
            {
                var command = new CreateFlashcardCommand
                {
                    FlashcardSetId = _flashcardSetId!.Value,
                    Question = question,
                    Answer = answer,
                    Hint = hint,
                    Difficulty = difficulty
                };

                if (OnCreatedFlashcard is null)
                {
                    _messageDialogService.ShowError(
                        "Błąd",
                        "Nie podpięto żadnego zdarzenia tworzenia fiszki.",
                        this);

                    return;
                }

                var result = OnCreatedFlashcard(
                    this,
                    command,
                    _flashcardSetId!.Value);

                if (result)
                    Close();
            }
            else
            {
                var command = new UpdateFlashcardCommand
                {
                    Question = question,
                    Answer = answer,
                    Hint = hint,
                    Difficulty = difficulty
                };

                if (OnEditedFlashcard is null)
                {
                    _messageDialogService.ShowError(
                        "Błąd",
                        "Nie podpięto żadnego zdarzenia edycji fiszki.",
                        this);

                    return;
                }

                var result = OnEditedFlashcard(
                    this,
                    command,
                    _flashcardId!.Value);

                if (result)
                    Close();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private sealed class DifficultyLevelItem
        {
            public DifficultyLevel Value { get; set; }
            public string Name { get; set; }
        }
    }
}