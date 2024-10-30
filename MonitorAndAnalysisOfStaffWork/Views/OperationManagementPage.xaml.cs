using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Окно управление операциями
    /// </summary>
    public partial class OperationManagementPage : Page
    {
        /// <summary>
        /// Результат поиска деталей
        /// </summary>
        public ObservableCollection<DetailEntity> FilteredDetails { get; set; } = [];

        /// <summary>
        /// Результат поиска деталей
        /// </summary>
        public ObservableCollection<OperationEntity> FilteredOperations { get; set; } = [];

        /// <summary>
        /// Сервис по управлению операциями
        /// </summary>
        private readonly OperationService ServiceOperation;

        /// <summary>
        /// Сервис по управлению деталями
        /// </summary>
        private readonly DetailService ServiceDetail;

        /// <summary>
        /// Окно управление операциями
        /// </summary>
        /// <exception cref="Exception"></exception>
        public OperationManagementPage()
        {
            InitializeComponent();
            ServiceOperation = App.AppHost.Services.GetService<OperationService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(OperationService)}");
            ServiceDetail = App.AppHost.Services.GetService<DetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(DetailService)}");
            DataContext = this;
        }

        /// <summary>
        /// Обновление списка операций
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        private async Task UpdateOperations(DetailEntity detail)
        {
            try
            {
                FilteredOperations.Clear();
                foreach (OperationEntity operation in await ServiceOperation.ListByDetail(detail.Id))
                {
                    FilteredOperations.Add(operation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие на кнопке - Добавить операцию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            DetailEntity? detail = DetailListView.SelectedItem as DetailEntity;
            if (detail == null)
            {
                MessageBox.Show("Деталь не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                OperationCreateWindow window = new (detail);
                window.ShowDialog();
                await UpdateOperations(detail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обработчик изменения текста в поисковом поле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DetailSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = DetailSearchBox.Text;
                List<DetailEntity> details = [];
                if (searchText.Length >= 3)
                {
                    details = await ServiceDetail.ListBySearch(searchText);
                }
                else
                {
                    details = await ServiceDetail.List();
                }
                await ClearDetailListView();
                foreach (DetailEntity detail in details)
                {
                    FilteredDetails.Add(detail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие на выбор детали из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DetailListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DetailListView.SelectedItem == null) return;
            DetailEntity? detail = DetailListView.SelectedItem as DetailEntity 
                ?? throw new Exception("Не удалось получить деталь (DetailListView_SelectionChanged)");
            try
            {
                await UpdateOperations(detail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Событие на кнопке - Удалить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            OperationEntity operation = button?.Tag as OperationEntity
                ?? throw new Exception("Не удалось получить операцию (DeleteButton_Click)");
            try
            {
                await ServiceOperation.Delete(operation.Id);
                FilteredOperations.Remove(operation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await ClearDetailListView();
            List<DetailEntity> details = await ServiceDetail.List();
            foreach (DetailEntity detail in details)
            {
                FilteredDetails.Add(detail);
            }

        }

        private async Task ClearDetailListView()
        {
            DetailListView.SelectedItem = null;
            FilteredDetails.Clear();
        }
    }
}
