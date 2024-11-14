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
using System.Windows.Shapes;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Логика взаимодействия для CreateWorkTimeLogWindow.xaml
    /// </summary>
    public partial class WorkTimeLogCreateWindow : Window
    {
        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        private EmployeeEntity Employee {  get; set; }

        /// <summary>
        /// Сервис по управлению рабочем временем сотрудника
        /// </summary>
        private readonly WorkTimeLogService ServiceWorkTimeLog;

        public WorkTimeLogCreateWindow(EmployeeEntity employee)
        {
            ServiceWorkTimeLog = App.AppHost?.Services.GetService<WorkTimeLogService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(WorkTimeLogService)}");

            InitializeComponent();
            Employee = employee;
            EmployeeFullNameTextBlock.Text = Employee.FullName;
        }

        /// <summary>
        /// Событие по кнопке - Сохранить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Дата не выбрана", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime date = DateDatePicker.SelectedDate.Value;
            if (string.IsNullOrEmpty(HoursTextBox.Text) || int.TryParse(HoursTextBox.Text, out int hours) == false)
            {
                MessageBox.Show("Часы не заданы или имеют неверный формат", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(MinutesTextBox.Text) || int.TryParse(MinutesTextBox.Text, out int minutes) == false)
            {
                MessageBox.Show("Минуты не заданы или имеют неверный формат", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                TimeSpan hoursWorked = new TimeSpan(hours, minutes, 0);
                WorkTimeLogEntity workTimeLog = await ServiceWorkTimeLog.Create(date, hoursWorked, Employee.Id);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие по кнопке - Отмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Обработка нажатия клавиш, чтобы разрешить удаление и использование стрелок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Разрешаем ввод только цифр
            e.Handled = !IsTextNumeric(e.Text);
        }

        /// <summary>
        /// Метод для проверки, что ввод является числом
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsTextNumeric(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Обработка нажатия клавиш, чтобы разрешить удаление и использование стрелок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumericTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.Left || e.Key == Key.Right)
            {
                // Разрешаем клавиши удаления и стрелки
                e.Handled = false;
            }
            else
            {
                e.Handled = !char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key));
            }
        }

    }
}
