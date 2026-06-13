using QuizMaster.Contracts.Commands.Learning;
using QuizMaster.Contracts.Exceptions;
using QuizMaster.Core.Dto;
using QuizMaster.Wpf.Interfaces;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace QuizMaster.Wpf.Windows
{
    public partial class LearningSessionWindow : Window
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageDialogService _messageDialogService;

        private int _sessionId;

        private LearningSessionDto _session;
        private LearningFlashcardDto _currentFlashcard;
        private bool _isHintVisible;

        private bool _isAnswerVisible;
        private bool _isFlipping;

        public LearningSessionWindow(
            IApiClient apiClient,
            IMessageDialogService messageDialogService)
        {
            InitializeComponent();

            _apiClient = apiClient;
            _messageDialogService = messageDialogService;

            SetAnswerButtonsEnabled(false);
        }

        public async Task InitializeAsync(int sessionId)
        {
            _sessionId = sessionId;

            await LoadSessionAsync();
            await LoadNextFlashcardAsync();
        }

        private async Task LoadSessionAsync()
        {
            _session = await _apiClient.GetAsync<LearningSessionDto>(
                $"api/learning-session/{_sessionId}");

            FlashcardSetNameTextBlock.Text = _session.FlashcardSetName;

            UpdateProgress(
                _session.ReviewedFlashcardsCount,
                _session.TotalFlashcardsCount);
        }

        private async Task LoadNextFlashcardAsync()
        {
            try
            {
                _currentFlashcard = await _apiClient.GetAsync<LearningFlashcardDto>(
                    $"api/learning-session/{_sessionId}/next-flashcard");

                _isAnswerVisible = false;

                CardSideTextBlock.Text = "Pytanie";
                CardTextBlock.Text = _currentFlashcard.Question;

                _isHintVisible = false;

                HintTextBlock.Visibility = Visibility.Collapsed;
                HintTextBlock.Text = string.Empty;

                ShowHintButton.Visibility = string.IsNullOrWhiteSpace(_currentFlashcard.Hint)
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                ShowHintButton.Content = "Podpowiedź";

                SetAnswerButtonsEnabled(false);
            }
            catch (LearningSessionExhaustedException)
            {
                ShowSessionFinished();
            }
        }

        private void CardBorder_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_currentFlashcard == null)
                return;

            if (_isFlipping)
                return;

            FlipCard();
        }

        private void ShowHintButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFlashcard == null)
                return;

            if (string.IsNullOrWhiteSpace(_currentFlashcard.Hint))
                return;

            _isHintVisible = !_isHintVisible;

            if (_isAnswerVisible)
            {
                CardSideTextBlock.Text = "Odpowiedź";
                CardTextBlock.Text = _currentFlashcard.Answer;

                ShowHintButton.Visibility = Visibility.Collapsed;
                HintTextBlock.Visibility = Visibility.Visible;
                HintTextBlock.Text = "Oceń, czy odpowiedziałeś poprawnie.";

                SetAnswerButtonsEnabled(true);
            }
            else
            {
                CardSideTextBlock.Text = "Pytanie";
                CardTextBlock.Text = _currentFlashcard.Question;

                _isHintVisible = false;

                HintTextBlock.Visibility = Visibility.Collapsed;
                HintTextBlock.Text = string.Empty;

                ShowHintButton.Visibility = string.IsNullOrWhiteSpace(_currentFlashcard.Hint)
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                ShowHintButton.Content = "Podpowiedź";

                SetAnswerButtonsEnabled(false);
            }
        }

        private void FlipCard()
        {
            _isFlipping = true;

            var hideAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(130)
            };

            hideAnimation.Completed += (_, _) =>
            {
                _isAnswerVisible = !_isAnswerVisible;

                if (_isAnswerVisible)
                {
                    CardSideTextBlock.Text = "Odpowiedź";
                    CardTextBlock.Text = _currentFlashcard.Answer;
                    HintTextBlock.Text = "Oceń, czy odpowiedziałeś poprawnie.";
                    SetAnswerButtonsEnabled(true);
                }
                else
                {
                    CardSideTextBlock.Text = "Pytanie";
                    CardTextBlock.Text = _currentFlashcard.Question;

                    HintTextBlock.Text = string.IsNullOrWhiteSpace(_currentFlashcard.Hint)
                        ? "Kliknij kartę, aby zobaczyć odpowiedź."
                        : $"Podpowiedź: {_currentFlashcard.Hint}";

                    SetAnswerButtonsEnabled(false);
                }

                var showAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(130)
                };

                showAnimation.Completed += (_, _) =>
                {
                    _isFlipping = false;
                };

                CardScaleTransform.BeginAnimation(
                    System.Windows.Media.ScaleTransform.ScaleXProperty,
                    showAnimation);
            };

            CardScaleTransform.BeginAnimation(
                System.Windows.Media.ScaleTransform.ScaleXProperty,
                hideAnimation);
        }

        private async void CorrectButton_Click(object sender, RoutedEventArgs e)
        {
            await AnswerAsync(true);
        }

        private async void WrongButton_Click(object sender, RoutedEventArgs e)
        {
            await AnswerAsync(false);
        }

        private async Task AnswerAsync(bool isCorrect)
        {
            if (_currentFlashcard == null)
                return;

            try
            {
                SetAnswerButtonsEnabled(false);

                var command = new AnswerFlashcardCommand
                {
                    FlashcardId = _currentFlashcard.Id,
                    IsCorrect = isCorrect
                };

                var result = await _apiClient.PostAsync<AnswerFlashcardCommand, AnswerFlashcardResultDto>(
                    $"api/learning-session/{_sessionId}/answer",
                    command);

                UpdateProgress(
                    result.ReviewedFlashcardsCount,
                    _session.TotalFlashcardsCount);

                if (result.SessionFinished)
                {
                    ShowSessionFinished();
                    return;
                }

                await LoadNextFlashcardAsync();
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    this);

                SetAnswerButtonsEnabled(true);
            }
        }

        private void UpdateProgress(int reviewed, int total)
        {
            SessionProgressBar.Maximum = total <= 0 ? 1 : total;
            SessionProgressBar.Value = reviewed;

            ProgressTextBlock.Text = $"{reviewed} / {total} fiszek";
        }

        private void ShowSessionFinished()
        {
            _currentFlashcard = null;

            CardSideTextBlock.Text = "Koniec";
            CardTextBlock.Text = "Sesja zakończona 🎉";

            ShowHintButton.Visibility = Visibility.Collapsed;

            HintTextBlock.Visibility = Visibility.Visible;
            HintTextBlock.Text = "Przerobiłeś wszystkie fiszki z tego zestawu.";

            SetAnswerButtonsEnabled(false);
        }

        private void SetAnswerButtonsEnabled(bool enabled)
        {
            CorrectButton.IsEnabled = enabled;
            WrongButton.IsEnabled = enabled;

            CorrectButton.Opacity = enabled ? 1 : 0.55;
            WrongButton.Opacity = enabled ? 1 : 0.55;
        }
    }
}