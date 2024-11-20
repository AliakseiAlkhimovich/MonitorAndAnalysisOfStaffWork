using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System.Windows;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Окно создания и редактирования сотрудника
    /// </summary>
    public partial class EmployeeCreateAndUpdateWindow : Window
    {
        /// <summary>
        /// Обновляемая запись сотрудника
        /// </summary>
        private EmployeeEntity? UpdateEmployee { get; set; } = null;

        /// <summary>
        /// Сервис по управлению сотрудниками
        /// </summary>
        private readonly EmployeeService ServiceEmployee;

        public EmployeeCreateAndUpdateWindow()
        {
            InitializeComponent();
            ServiceEmployee = App.AppHost?.Services.GetService<EmployeeService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(EmployeeService)}");
            Load();
        }
        public EmployeeCreateAndUpdateWindow(EmployeeEntity updateEmployee)
        {
            InitializeComponent();
            ServiceEmployee = App.AppHost?.Services.GetService<EmployeeService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(EmployeeService)}");
            UpdateEmployee = updateEmployee;
            Load();
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <returns></returns>
        private void Load()
        {
            if (UpdateEmployee != null)
            {
                FullNameTextBox.Text = UpdateEmployee.FullName;
                PositionBox.Text = UpdateEmployee.Position;
                NumberBox.Text = UpdateEmployee.Number;
            }
        }


        /// <summary>
        /// Создать или обновить сотрудника
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FullNameTextBox.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(PositionBox.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(NumberBox.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (UpdateEmployee == null)
                {
                    await ServiceEmployee.Create(FullNameTextBox.Text, PositionBox.Text, NumberBox.Text);
                }
                else
                {
                    await ServiceEmployee.Update(UpdateEmployee.Id, FullNameTextBox.Text, PositionBox.Text, NumberBox.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();

        }


        /// <summary>
        /// Событие на кнопку "Отмена"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
