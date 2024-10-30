using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
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
    /// Окно управления изготовленными деталями
    /// </summary>
    public partial class ManufacturedDetailManagementPage : Page
    {
        /// <summary>
        /// Результат поиска деталей
        /// </summary>
        public ObservableCollection<ManufacturedDetailEntity> FilteredDetails { get; set; } = [];



        /// <summary>
        /// Строка поиска
        /// </summary>
        private string Search { get; set; } = "";

        /// <summary>
        /// Сервис по управлению деталями
        /// </summary>
        private readonly ManufacturedDetailService ServiceManufacturedDetail;

        /// <summary>
        /// Сервис по генерации и создание отчетов
        /// </summary>
        private readonly OrderService ServiceOrder;



        /// <summary>
        /// Окно управления изготовленными деталями
        /// </summary>
        /// <exception cref="Exception"></exception>
        public ManufacturedDetailManagementPage()
        {
            ServiceManufacturedDetail = App.AppHost.Services.GetService<ManufacturedDetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(ManufacturedDetailService)}");
            ServiceOrder = App.AppHost.Services.GetService<OrderService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(OrderService)}");
            InitializeComponent();
            DataContext = this;
            Load();
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        public async void Load()
        {
            List<ManufacturedDetailEntity> details = [];
            if (string.IsNullOrEmpty(Search) || Search.Length < 3)
            {
                details = await ServiceManufacturedDetail.List();
            }
            else
            {
                details = await ServiceManufacturedDetail.ListBySearch(Search);
            }

            FilteredDetails.Clear();
            foreach (ManufacturedDetailEntity detail in details)
            {
                FilteredDetails.Add(detail);
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
                Search = searchText;
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Собтие на кнопку - Добавить деталь
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ManufacturedDetailCreateAndUpdatePage window = new();
            window.ShowDialog();
            Load();
        }

        private async void OrderButton_Click(Object sender, RoutedEventArgs e)
        {
            if (StartDateDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Не выбрана начальная дата", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime startDate = StartDateDatePicker.SelectedDate.Value;
            if (EndDateDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Не выбрана конечная дата", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime endDate = EndDateDatePicker.SelectedDate.Value;
            try
            {


                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Сохранить отчет",
                    FileName = $"Список изготовленных деталей за {startDate.ToString("dd.MM.yyyy")} - {endDate.ToString("dd.MM.yyyy")}.xlsx"
                };


                // Если пользователь выбрал место и имя файла
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    // Генерация и сохранение Excel файла
                    await ServiceOrder.ManufacturedDetail(filePath, startDate, endDate);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// Событие на кнопку "Просмотр"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewDataGridButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            if (button.DataContext is not ManufacturedDetailEntity manufacturedDetail) return;

            try
            {
                ManufacturedDetailCreateAndUpdatePage window = new(manufacturedDetail);
                window.ShowDialog();
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие на кнопку "Удалить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteDataGridButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            if (button.DataContext is not ManufacturedDetailEntity manufacturedDetail) return;

            try
            {
                if (MessageBox.Show($"Удалить деталь #{manufacturedDetail.DetailName} от {manufacturedDetail.ManufactureDateString}?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await ServiceManufacturedDetail.Delete(manufacturedDetail.Id);
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
