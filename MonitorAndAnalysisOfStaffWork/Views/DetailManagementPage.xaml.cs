using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System.Windows;
using System.Windows.Controls;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Окно управления деталями
    /// </summary>
    public partial class DetailManagementPage : Page
    {
        /// <summary>
        /// Сервис по управлению деталями
        /// </summary>
        private readonly DetailService ServiceDetail;

        /// <summary>
        /// Сервис по генерации и создание отчетов
        /// </summary>
        private readonly OrderService ServiceOrder;

        /// <summary>
        /// Окно управления деталями
        /// </summary>
        /// <exception cref="Exception"></exception>
        public DetailManagementPage()
        {
            ServiceDetail = App.AppHost.Services.GetService<DetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(DetailService)}");
            ServiceOrder = App.AppHost.Services.GetService<OrderService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(OrderService)}");
            InitializeComponent();
            Load();
        }

        /// <summary>
        /// Событие по кнопке - Добавить деталь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            DetailCreateAndUpdateWindow window = new();
            window.ShowDialog();
            Load();
        }

        /// <summary>
        /// Событие по кнопке - Сформировать отчет
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Сохранить отчет",
                    FileName = $"Список деталей.xlsx"
                };

                // Если пользователь выбрал место и имя файла
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    // Генерация и сохранение Excel файла
                    await ServiceOrder.DetailAndOperation(filePath);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        private async void Load()
        {
            List<DetailEntity> employees = await ServiceDetail.List();
            DetailDataGrid.ItemsSource = employees;
        }


        /// <summary>
        /// Событие на кнопку "Просмотр информации"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewDataGridButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            if (button.DataContext is not DetailEntity detail) return;

            try
            {
                DetailCreateAndUpdateWindow window = new(detail);
                window.ShowDialog();
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие на кнопку "Удаление"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteDataGridButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            if (button.DataContext is not DetailEntity detail) return;

            try
            {
                if (MessageBox.Show($"Удалить деталь #{detail.Number} {detail.Name} - {detail.Designation}?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await ServiceDetail.Delete(detail.Id);
                    Load();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
