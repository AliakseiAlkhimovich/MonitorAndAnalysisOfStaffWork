using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System.Windows;
using System.Windows.Controls;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Логика взаимодействия для OperationTypeManagementPage.xaml
    /// </summary>
    public partial class OperationTypeManagementPage : Page
    {
        /// <summary>
        /// Сервис по управлению операциями
        /// </summary>
        private readonly OperationService ServiceOperation;
        public OperationTypeManagementPage()
        {
            InitializeComponent();
            ServiceOperation = App.AppHost?.Services.GetService<OperationService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(OperationService)}");
        }




        /// <summary>
        /// Событие по кнопке - Добавить деталь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(NameTextBox.Text))
                {
                    throw new Exception($"Введите наименование типа операции");
                }
                await ServiceOperation.CreateOperationType(NameTextBox.Text);
                NameTextBox.Text = "";
                await Load();
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
            if (button.DataContext is not OperationTypeEntity operationType) return;

            try
            {
                if (MessageBox.Show($"Удалить тип операции #{operationType.Id} {operationType.Name}?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await ServiceOperation.DeleteOperationType(operationType.Id);
                    await Load();
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
        private async Task Load()
        {
            List<OperationTypeEntity> operationTypes = await ServiceOperation.ListOperationType();
            OperationTypeDataGrid.ItemsSource = operationTypes;
        }

        /// <summary>
        /// Загрузка данных при загрузке окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await Load();
        }
    }
}
