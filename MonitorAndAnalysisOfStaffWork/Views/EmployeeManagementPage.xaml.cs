using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System;
using System.Collections.Generic;
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

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployeeManagementPage.xaml
    /// </summary>
    public partial class EmployeeManagementPage : Page
    {
        /// <summary>
        /// Сервис по управлению сотрудниками
        /// </summary>
        private readonly EmployeeService ServiceEmployee;

        public EmployeeManagementPage()
        {
            InitializeComponent();
            ServiceEmployee = App.AppHost.Services.GetService<EmployeeService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(EmployeeService)}");
            Load();
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        private async void Load()
        {
            List<EmployeeEntity> employees = await ServiceEmployee.List();
            EmployeeDataGrid.ItemsSource = employees;
        }

        /// <summary>
        /// Событие на кнопку "Добавить сотрудника"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeCreateAndUpdateWindow window = new ();
            window.ShowDialog();
            Load();

        }



        /// <summary>
        /// Событие на кнопку "Просмотр информации"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewDataGridButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            if (button.DataContext is not EmployeeEntity employee) return;

            try
            {
                EmployeeCreateAndUpdateWindow window = new (employee);
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
            if (button.DataContext is not EmployeeEntity employee) return;

            try
            {
                if (MessageBox.Show($"Удалить сотрудника #{employee.Number} {employee.FullName}?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await ServiceEmployee.Delete(employee.Id);
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
