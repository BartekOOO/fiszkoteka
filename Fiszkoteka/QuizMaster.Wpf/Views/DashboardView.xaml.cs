using Microsoft.Extensions.DependencyInjection;
using QuizMaster.Contracts.Dto;
using QuizMaster.Wpf.Interfaces;
using QuizMaster.Wpf.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizMaster.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        private readonly IApiClient _apiClient;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IServiceProvider _serviceProvider;

        public DashboardView(
            IApiClient apiClient,
            IMessageDialogService messageDialogService,
            IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _apiClient = apiClient;
            _messageDialogService = messageDialogService;

            Loaded += DashboardView_Loaded;
            _serviceProvider = serviceProvider;
        }

        private async void DashboardView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= DashboardView_Loaded;
            await LoadDashboardAsync();
        }

        private async Task LoadDashboardAsync()
        {
            try
            {
                var dashboard = await _apiClient.GetAsync<MainDashboardDto>(
                    "api/dashboard");

                DataContext = dashboard;
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    Window.GetWindow(this));
            }
        }

        private async void CreateFlashcardSet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var createFlashcardSetWindow = _serviceProvider.GetRequiredService
                    <CreateFlashcardSetWindow>();

                createFlashcardSetWindow.Owner = Window.GetWindow(this);
                createFlashcardSetWindow.Closed += (_, _) => Window.GetWindow(this).Activate();

                createFlashcardSetWindow.OnCreatedFlashcardSet += async (sender, id) =>
                {
                    await LoadDashboardAsync();
                    var editWindow = _serviceProvider.GetRequiredService
                        <EditFlashcardSetWindow>();
                    editWindow.Owner = Window.GetWindow(this);

                    editWindow.Saved += async (s) =>
                    {
                        await LoadDashboardAsync();
                    };

                    await editWindow.LoadAsync(id);
                    editWindow.Show();
                };

                createFlashcardSetWindow.Show();
            }
            catch (Exception ex)
            {
                _messageDialogService.ShowError(
                    "Błąd",
                    ex.Message,
                    Window.GetWindow(this));
            }
        }
    }
}
