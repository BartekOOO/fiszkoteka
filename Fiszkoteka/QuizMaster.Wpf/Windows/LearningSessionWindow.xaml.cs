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

        private bool _isAnswerVisible;
        private bool _isFlipping;
        private bool _isHintVisible;
        private bool _isSessionFinished;

        public LearningSessionWindow(
            IApiClient apiClient,
            IMessageDialogService messageDialogService)
        {
            InitializeComponent();

            _apiClient = apiClient;
            _messageDialogService = messageDialogService;

            SetAnswerButtonsEnabled(false);

            ShowHintButton.Visibility = Visibility.Collapsed;
            HintPanel.Visibility = Visibility.Collapsed;
            FinishButton.Visibility = Visibility.Collapsed;
        }

        public async Task InitializeAsync(int sessionId)
        {
            _sessionId = sessionId;

            await LoadSessionAsync();

            if (_session.IsFinished)
            {
                ShowSessionFinished();
                return;
            }

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
                _isHintVisible = false;
                _isSessionFinished = false;

                CardSideTextBlock.Text = "Pytanie";
                CardTextBlock.Text = _currentFlashcard.Question;
                CardFooterTextBlock.Text = "Kliknij kartę, aby zobaczyć odpowiedź.";

                HideHint();

                if (string.IsNullOrWhiteSpace(_currentFlashcard.Hint))
                {
                    ShowHintButton.Visibility = Visibility.Visible;
                    ShowHintButton.IsEnabled = false;
                    ShowHintButton.Content = "Brak podpowiedzi";
                }
                else
                {
                    ShowHintButton.Visibility = Visibility.Visible;
                    ShowHintButton.IsEnabled = true;
                    ShowHintButton.Content = "Podpowiedź";
                }

                FinishButton.Visibility = Visibility.Collapsed;

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

            if (_isSessionFinished)
                return;

            FlipCard();
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
                    ShowAnswerSide();
                else
                    ShowQuestionSide();

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

        private void ShowQuestionSide()
        {
            if (_currentFlashcard == null)
                return;

            CardSideTextBlock.Text = "Pytanie";
            CardTextBlock.Text = _currentFlashcard.Question;
            CardFooterTextBlock.Text = "Kliknij kartę, aby zobaczyć odpowiedź.";

            HideHint();

            if (string.IsNullOrWhiteSpace(_currentFlashcard.Hint))
            {
                ShowHintButton.Visibility = Visibility.Visible;
                ShowHintButton.IsEnabled = false;
                ShowHintButton.Content = "Brak podpowiedzi";
            }
            else
            {
                ShowHintButton.Visibility = Visibility.Visible;
                ShowHintButton.IsEnabled = true;
                ShowHintButton.Content = "Podpowiedź";
            }

            SetAnswerButtonsEnabled(false);
        }

        private void ShowAnswerSide()
        {
            if (_currentFlashcard == null)
                return;

            CardSideTextBlock.Text = "Odpowiedź";
            CardTextBlock.Text = _currentFlashcard.Answer;
            CardFooterTextBlock.Text = "Oceń, czy odpowiedziałeś poprawnie.";

            HideHint();

            ShowHintButton.Visibility = Visibility.Collapsed;
            ShowHintButton.IsEnabled = false;

            SetAnswerButtonsEnabled(true);
        }

        private void ShowHintButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (_currentFlashcard == null)
                return;

            if (_isSessionFinished)
                return;

            if (_isAnswerVisible)
                return;

            if (string.IsNullOrWhiteSpace(_currentFlashcard.Hint))
                return;

            if (_isHintVisible)
            {
                HideHint();
                ShowHintButton.Content = "Podpowiedź";
                return;
            }

            _isHintVisible = true;
            HintTextBlock.Text = _currentFlashcard.Hint;
            HintPanel.Visibility = Visibility.Visible;
            ShowHintButton.Content = "Ukryj podpowiedź";
        }

        private void HideHint()
        {
            _isHintVisible = false;
            HintTextBlock.Text = string.Empty;
            HintPanel.Visibility = Visibility.Collapsed;
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

            if (!_isAnswerVisible)
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
            _isSessionFinished = true;
            _isAnswerVisible = false;
            _isHintVisible = false;
            _currentFlashcard = null;

            CardSideTextBlock.Text = "Koniec";
            CardTextBlock.Text = "Sesja zakończona 🎉";
            CardFooterTextBlock.Text = "Przerobiłeś wszystkie fiszki z tego zestawu.";

            HideHint();

            ShowHintButton.Visibility = Visibility.Collapsed;
            ShowHintButton.IsEnabled = false;

            SetAnswerButtonsEnabled(false);

            FinishButton.Visibility = Visibility.Visible;

            if (_session != null)
            {
                UpdateProgress(
                    _session.TotalFlashcardsCount,
                    _session.TotalFlashcardsCount);
            }
        }

        private void SetAnswerButtonsEnabled(bool enabled)
        {
            CorrectButton.IsEnabled = enabled;
            WrongButton.IsEnabled = enabled;

            CorrectButton.Opacity = enabled ? 1 : 0.55;
            WrongButton.Opacity = enabled ? 1 : 0.55;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}