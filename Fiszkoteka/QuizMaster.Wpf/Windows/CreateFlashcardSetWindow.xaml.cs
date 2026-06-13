using QuizMaster.Contracts.Commands.FlashcardSets;
using QuizMaster.Core.Models;
using QuizMaster.Wpf.Delegates;
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
    /// Logika interakcji dla klasy CreateFlashcardSetWindow.xaml
    /// </summary>
    public partial class CreateFlashcardSetWindow : Window
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageDialogService _messageDialogService;

        public event CreateFlashcardSetHandler OnCreatedFlashcardSet;

        public CreateFlashcardSetWindow(
            IApiClient apiClient,
            IMessageDialogService messageDialogService)
        {
            InitializeComponent();

            _apiClient = apiClient;
            _messageDialogService = messageDialogService;

            Loaded += CreateFlashcardSetWindow_Loaded;
        }

        private async void CreateFlashcardSetWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= CreateFlashcardSetWindow_Loaded;

            await LoadCategoriesAsync();
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                var categories = await _apiClient.GetAsync<List<Category>>(
                    "api/category");

                CategoryComboBox.ItemsSource = categories;

                if (categories.Count > 0)
                    CategoryComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    this);
            }
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameTextBox.Text?.Trim();
            var description = DescriptionTextBox.Text?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    "Podaj nazwę zestawu.",
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

            var categoryId = (int)CategoryComboBox.SelectedValue;

            var command = new CreateFlashcardSetCommand
            {
                Name = name,
                Description = description,
                CategoryId = categoryId,
            };

            try
            {
                var createdSet = await _apiClient.PostAsync<CreateFlashcardSetCommand, FlashcardSet>(
                    "api/flashcardset",
                    command);

                OnCreatedFlashcardSet?.Invoke(this, createdSet.Id);
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
    }
}
