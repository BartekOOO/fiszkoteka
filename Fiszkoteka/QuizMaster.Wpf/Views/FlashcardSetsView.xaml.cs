using Microsoft.Extensions.DependencyInjection;
using QuizMaster.Contracts.Commands.Learning;
using QuizMaster.Contracts.Dto;
using QuizMaster.Core.Dto;
using QuizMaster.Core.Models;
using QuizMaster.Wpf.Interfaces;
using QuizMaster.Wpf.ViewModels;
using QuizMaster.Wpf.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMaster.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy FlashcardSetsView.xaml
    /// </summary>
    public partial class FlashcardSetsView : UserControl
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IServiceProvider _serviceProvider;

        private readonly ObservableCollection<FlashcardSetListItemViewModel> _sets;

        public FlashcardSetsView(IMessageDialogService messageDialogService, IApiClient apiClient, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _messageDialogService = messageDialogService;
            _apiClient = apiClient;
            _serviceProvider = serviceProvider;

            _sets = new ObservableCollection<FlashcardSetListItemViewModel>();
            FlashcardSetsItemsControl.ItemsSource = _sets;

            Loaded += FlashcardSetsView_Loaded;
        }

        private async void FlashcardSetsView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= FlashcardSetsView_Loaded;
            await LoadFlashcardSetsAsync();
        }

        private async Task LoadFlashcardSetsAsync()
        {
            try
            {
                var flashcardSets = await _apiClient.GetAsync<List<FlashcardSetListItemDto>>(
                    "api/flashcardset");

                _sets.Clear();

                foreach (var set in flashcardSets)
                {
                    _sets.Add(new FlashcardSetListItemViewModel(set));
                }

                UpdateEmptyState();
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    Window.GetWindow(this));
            }
        }

        private void CreateSetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var createFlashcardSetWindow = _serviceProvider.GetRequiredService
                    <CreateFlashcardSetWindow>();

                createFlashcardSetWindow.Owner = Window.GetWindow(this);
                createFlashcardSetWindow.Closed += (_, _) => Window.GetWindow(this).Activate();

                createFlashcardSetWindow.OnCreatedFlashcardSet += async (sender, id) =>
                {
                    await LoadFlashcardSetsAsync();
                };

                createFlashcardSetWindow.Show();
            }
            catch(Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    Window.GetWindow(this));
            }
        }

        private async void LearnButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Tag is not int flashcardSetId)
                return;

            try
            {
                var command = new StartLearningSessionCommand
                {
                    FlashcardSetId = flashcardSetId
                };

                var session = await _apiClient.PostAsync<StartLearningSessionCommand, LearningSessionDto>(
                    "api/learning-session/start",
                    command);

                var learningWindow = _serviceProvider
                    .GetRequiredService<LearningSessionWindow>();

                learningWindow.Owner = Window.GetWindow(this);
                learningWindow.Closed += (_, _) => Window.GetWindow(this).Activate();
                await learningWindow.InitializeAsync(session.Id);

                learningWindow.Show();
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    Window.GetWindow(this));
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Tag is not int flashcardSetId)
                return;

            try
            {
                var editWindow = _serviceProvider.GetRequiredService
                    <EditFlashcardSetWindow>();
                editWindow.Owner = Window.GetWindow(this);
                editWindow.Closed += (_, _) => Window.GetWindow(this).Activate();
                editWindow.Saved += async (s) =>
                {
                    await LoadFlashcardSetsAsync();
                };

                await editWindow.LoadAsync(flashcardSetId);
                editWindow.Show();

            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                        "Błąd",
                        ex.Message,
                        Window.GetWindow(this));
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Tag is not int flashcardSetId)
                return;

            try
            {
                await _apiClient.DeleteAsync(
                    $"api/flashcardset/{flashcardSetId}");

                var item = _sets.FirstOrDefault(x => x.Id == flashcardSetId);

                if (item != null)
                    _sets.Remove(item);

                UpdateEmptyState();
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    Window.GetWindow(this));
            }
        }

        public void UpdateEmptyState()
        {
            EmptyStateTextBlock.Visibility = _sets.Count == 0
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

    }
}

