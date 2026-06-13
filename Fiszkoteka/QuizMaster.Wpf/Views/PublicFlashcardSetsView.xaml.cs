using QuizMaster.Contracts.Dto;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMaster.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy PublicFlashcardSetsView.xaml
    /// </summary>
    public partial class PublicFlashcardSetsView : UserControl
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageDialogService _messageDialogService;

        private readonly ObservableCollection<PublicFlashcardSetListItemViewModel> _sets;

        public PublicFlashcardSetsView(
            IApiClient apiClient,
            IMessageDialogService messageDialogService)
        {
            InitializeComponent();

            _apiClient = apiClient;
            _messageDialogService = messageDialogService;

            _sets = new ObservableCollection<PublicFlashcardSetListItemViewModel>();

            PublicFlashcardSetsItemsControl.ItemsSource = _sets;

            Loaded += PublicFlashcardSetsView_Loaded;
        }

        private async void PublicFlashcardSetsView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= PublicFlashcardSetsView_Loaded;

            await LoadPublicSetsAsync();
        }

        private async Task LoadPublicSetsAsync()
        {
            try
            {
                var path = BuildPath();

                var publicSets = await _apiClient.GetAsync<List<FlashcardSetListItemDto>>(
                    path);

                _sets.Clear();

                foreach (var set in publicSets)
                {
                    _sets.Add(new PublicFlashcardSetListItemViewModel(set));
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

        private string BuildPath()
        {
            var builder = new StringBuilder("api/flashcardset/public");

            var query = new List<string>();

            var userName = UserNameFilterTextBox.Text?.Trim();
            var category = CategoryFilterTextBox.Text?.Trim();

            if (!string.IsNullOrWhiteSpace(userName))
            {
                query.Add($"byUserName={Uri.EscapeDataString(userName)}");
            }

            if (!string.IsNullOrWhiteSpace(category))
            {
                query.Add($"byCategory={Uri.EscapeDataString(category)}");
            }

            if (query.Count > 0)
            {
                builder.Append('?');
                builder.Append(string.Join("&", query));
            }

            return builder.ToString();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadPublicSetsAsync();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadPublicSetsAsync();
        }

        private async void ClearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            UserNameFilterTextBox.Text = string.Empty;
            CategoryFilterTextBox.Text = string.Empty;

            await LoadPublicSetsAsync();
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.Tag is not int flashcardSetId)
                return;

            try
            {
                button.IsEnabled = false;
                button.Content = "Pobieranie...";

                await _apiClient.PostAsync<CopiedFlashcardSetDto>(
                    $"api/flashcardset/{flashcardSetId}/Copy");

                button.Content = "Pobrano";
            }
            catch (Exception ex)
            {
                button.IsEnabled = true;
                button.Content = "Pobierz";

                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    Window.GetWindow(this));
            }
        }

        private void UpdateEmptyState()
        {
            EmptyStateTextBlock.Visibility = _sets.Count == 0
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
