using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32; 

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Логика взаимодействия для страницы "Учет рабочего времени сотрудников"
    /// </summary>
    public partial class WorkTimeLogManagementPage : Page
    {
        /// <summary>
        /// Сервис по управлению сотрудниками
        /// </summary>
        private readonly EmployeeService ServiceEmployee;

        /// <summary>
        /// Сервис по управлению сотрудниками
        /// </summary>
        private readonly WorkTimeLogService ServiceWorkTimeLog;

        /// <summary>
        /// Сервис по генерации и создание отчетов
        /// </summary>
        private readonly OrderService ServiceOrder;

        /// <summary>
        /// Результат поиска сотрудников по табельному номеру
        /// </summary>
        public ObservableCollection<EmployeeEntity> FilteredEmployees { get; set; } = [];

        /// <summary>
        /// Результат поиска рабочего времени сотрудника
        /// </summary>
        public ObservableCollection<WorkTimeLogEntity> WorkTimeLogs { get; set; } = [];


        /// <summary>
        /// Логика взаимодействия для страницы "Учет рабочего времени сотрудников"
        /// </summary>
        /// <exception cref="Exception"></exception>
        public WorkTimeLogManagementPage()
        {
            InitializeComponent();
            ServiceEmployee = App.AppHost.Services.GetService<EmployeeService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(EmployeeService)}");
            ServiceWorkTimeLog = App.AppHost.Services.GetService<WorkTimeLogService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(WorkTimeLogService)}");

            ServiceOrder = App.AppHost.Services.GetService<OrderService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(OrderService)}");

            DataContext = this;


            int currentYear = DateTime.Now.Year;
            for (int year = 1990; year <= currentYear; year++)
            {
                SelectYearComboBox.Items.Add(year);
            }
            // Устанавливаем значение по умолчанию - текущий год
            SelectYearComboBox.SelectedItem = currentYear;

        }

        /// <summary>
        /// Обработчик изменения текста в поисковом поле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EmployeeSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = EmployeeSearchBox.Text;

            List<EmployeeEntity> employees = [];
            // Поиск начинается при вводе 3 и более символов
            if (searchText.Length >= 3)
            {
                 employees = await ServiceEmployee.ListByNumber(searchText);
            }
            else
            {
                employees = await ServiceEmployee.List();
            }
            ClearEmployeeListView();
            foreach (EmployeeEntity employee in employees)
            {
                FilteredEmployees.Add(employee);
            }
        }


        /// <summary>
        /// Событие на кнопку "Добавить отметку"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeEntity? employee = EmployeeListView.SelectedItem as EmployeeEntity;
            if (employee == null)
            {
                MessageBox.Show("Сотрудник не выбран", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            try
            {
                WorkTimeLogCreateWindow window = new(employee);
                window.ShowDialog();
                await UpdateWorkTimeLogListView(employee);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие на выбор сотрудника из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private async void EmployeeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EmployeeListView.SelectedItem == null) return;
            EmployeeEntity? employee = EmployeeListView.SelectedItem as EmployeeEntity ?? throw new Exception("Не удалось получить сотрудника (EmployeeListView_SelectionChanged)");
            try
            {
                await UpdateWorkTimeLogListView(employee);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Обновление данных в списке - Рабочее время сотрудника
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private async Task UpdateWorkTimeLogListView(EmployeeEntity employee)
        {
            try
            {
                WorkTimeLogs.Clear();
                foreach (WorkTimeLogEntity workTimeLog in await ServiceWorkTimeLog.ListByEmployee(employee.Id))
                {
                    WorkTimeLogs.Add(workTimeLog);
                }
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
            WorkTimeLogEntity workTimeLog = button?.Tag as WorkTimeLogEntity
                ?? throw new Exception("Не удалось получить рабочее время сотрудника (DeleteButton_Click)");
            try
            {
                await ServiceWorkTimeLog.Delete(workTimeLog.Id);
                WorkTimeLogs.Remove(workTimeLog);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // OrderButton_Click

        
        /// <summary>
        /// Событие на кнопку "Добавить отметку"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectYearComboBox.SelectedItem == null)
            {
                MessageBox.Show("Дата не выбрана", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int year = (int)SelectYearComboBox.SelectedItem;

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel Files|*.xlsx",
                    Title = "Сохранить отчет",
                    FileName = $"Табель учета рабочего времени работников за {year}.xlsx"
                };

                // Если пользователь выбрал место и имя файла
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;

                    // Генерация и сохранение Excel файла
                    await ServiceOrder.WorkTimeLog(filePath, year);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ClearEmployeeListView();
            List<EmployeeEntity> employees = await ServiceEmployee.List();
            foreach (EmployeeEntity employee in employees)
            {
                FilteredEmployees.Add(employee);
            }
        }



        private void ClearEmployeeListView()
        {
            EmployeeListView.SelectedItem = null;
            FilteredEmployees.Clear();
        }
    }

}
