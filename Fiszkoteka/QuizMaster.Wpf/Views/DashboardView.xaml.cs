using QuizMaster.Contracts.Dto;
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

        public DashboardView(
            IApiClient apiClient,
            IMessageDialogService messageDialogService)
        {
            InitializeComponent();

            _apiClient = apiClient;
            _messageDialogService = messageDialogService;

            Loaded += DashboardView_Loaded;
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
    }
}
