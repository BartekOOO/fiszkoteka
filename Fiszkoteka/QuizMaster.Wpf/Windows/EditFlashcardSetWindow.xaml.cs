using Microsoft.Extensions.DependencyInjection;
using QuizMaster.Contracts.Commands.Flashcards;
using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Core.Enums;
using QuizMaster.Core.Models;
using QuizMaster.Wpf.Delegates;
using QuizMaster.Wpf.Interfaces;
using QuizMaster.Wpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logika interakcji dla klasy EditFlashcardSetWindow.xaml
    /// </summary>
    public partial class EditFlashcardSetWindow : Window
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IServiceProvider _serviceProvider;

        private readonly ObservableCollection<FlashcardListItemViewModel> _flashcards;

        private int _flashcardSetId;
        private FlashcardSet _flashcardSet;

        public event EditedFlashcardSetHandler Saved;

        public EditFlashcardSetWindow(
            IApiClient apiClient,
            IMessageDialogService messageDialogService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _apiClient = apiClient;
            _messageDialogService = messageDialogService;

            _flashcards = new ObservableCollection<FlashcardListItemViewModel>();
            FlashcardsItemsControl.ItemsSource = _flashcards;
            _serviceProvider = serviceProvider;
        }

        public async Task LoadAsync(int flashcardSetId)
        {
            _flashcardSetId = flashcardSetId;

            try
            {
                await LoadCategoriesAsync();
                await LoadFlashcardSetAsync();
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    this);
            }
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _apiClient.GetAsync<List<Category>>(
                "api/category");

            CategoryComboBox.ItemsSource = categories;
        }

        private async Task LoadFlashcardSetAsync()
        {
            _flashcardSet = await _apiClient.GetAsync<FlashcardSet>(
                $"api/flashcardset/{_flashcardSetId}");

            NameTextBox.Text = _flashcardSet.Name;
            DescriptionTextBox.Text = _flashcardSet.Description;
            CategoryComboBox.SelectedValue = _flashcardSet.CategoryId;

            _flashcards.Clear();

            if (_flashcardSet.Flashcards != null)
            {
                foreach (var flashcard in _flashcardSet.Flashcards)
                {
                    _flashcards.Add(new FlashcardListItemViewModel(flashcard));
                }
            }

            UpdateEmptyState();
        }

        private async Task SaveAsync()
        {
            var name = NameTextBox.Text?.Trim();
            var description = DescriptionTextBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    "Nazwa zestawu nie może być pusta.",
                    this);

                return;
            }

            if (CategoryComboBox.SelectedValue == null)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    "Wybierz kategorię.",
                    this);

                return;
            }

            var command = new UpdateFlashcardSetCommand
            {
                Name = name,
                Description = description,
                CategoryId = (int)CategoryComboBox.SelectedValue
            };

            await _apiClient.PutAsync(
                $"api/flashcardset/{_flashcardSetId}",
                command);

            Saved?.Invoke(this);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SaveAsync();

                _messageDialogService.ShowError(
                    "Zapisano",
                    "Zestaw został zapisany.",
                    this);
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    this);
            }
        }

        private async void SaveAndCloseButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SaveAsync();
                Close();
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    this);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void AddFlashcardButton_Click(object sender, RoutedEventArgs e)
        {
            var window = _serviceProvider.GetRequiredService
                <EditFlashcardWindow>();

            window.Owner = this;

            window.ShowDialog();

            try
            {
                var command = new CreateFlashcardCommand
                {
                    FlashcardSetId = _flashcardSetId,
                    Question = window.Question,
                    Answer = window.Answer,
                    Hint = window.Hint,
                    Difficulty = window.Difficulty
                };

                var createdFlashcard = await _apiClient.PostAsync<CreateFlashcardCommand, Flashcard>(
                    "api/flashcard",
                    command);

                _flashcards.Add(new FlashcardListItemViewModel(createdFlashcard));

                UpdateEmptyState();

                Saved?.Invoke(this);
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    this);
            }
        }

        private async void EditFlashcardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Tag is not int flashcardId)
                return;

            var item = _flashcards.FirstOrDefault(x => x.Id == flashcardId);

            if (item == null)
                return;

            var window = new EditFlashcardWindow(
                item.Question,
                item.Answer,
                item.Hint,
                item.Difficulty);

            window.Owner = this;

            if (window.ShowDialog() != true)
                return;

            try
            {
                var command = new UpdateFlashcardCommand
                {
                    Question = window.Question,
                    Answer = window.Answer,
                    Hint = window.Hint,
                    Difficulty = window.Difficulty
                };

                await _apiClient.PutAsync(
                    $"api/flashcard/{flashcardId}",
                    command);

                item.Question = window.Question;
                item.Answer = window.Answer;
                item.Hint = window.Hint;
                item.Difficulty = window.Difficulty;

                FlashcardsItemsControl.Items.Refresh();

                Saved?.Invoke(this);
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    this);
            }
        }

        private async void DeleteFlashcardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Tag is not int flashcardId)
                return;

            try
            {
                await _apiClient.DeleteAsync(
                    $"api/flashcard/{flashcardId}");

                var item = _flashcards.FirstOrDefault(x => x.Id == flashcardId);

                if (item != null)
                    _flashcards.Remove(item);

                UpdateEmptyState();

                Saved?.Invoke(this);
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    this);
            }
        }

        private void UpdateEmptyState()
        {
            EmptyFlashcardsTextBlock.Visibility = _flashcards.Count == 0
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
