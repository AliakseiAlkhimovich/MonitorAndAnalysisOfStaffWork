using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System.Windows;
using System.Windows.Input;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Окно создания новой операции
    /// </summary>
    public partial class OperationCreateWindow : Window
    {
        /// <summary>
        /// Сервис по управлению операциями
        /// </summary>
        private readonly OperationService ServiceOperation;

        /// <summary>
        /// Выбранная деталь
        /// </summary>
        private DetailEntity SelectedDetail { get; set; }

        /// <summary>
        /// Окно создания новой операции
        /// </summary>
        /// <exception cref="Exception"></exception>
        public OperationCreateWindow(DetailEntity selectedDetail)
        {
            InitializeComponent();
            ServiceOperation = App.AppHost.Services.GetService<OperationService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(OperationService)}");
            SelectedDetail = selectedDetail;
            Load();
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

        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <returns></returns>
        private async void Load()
        {
            DetailNameTextBox.Text = $"{SelectedDetail.Number}.{SelectedDetail.Name}";
            List<OperationTypeEntity> operationTypes = await ServiceOperation.ListOperationType();
            OperationTypeComboBox.ItemsSource = operationTypes;
            OperationTypeComboBox.DisplayMemberPath = "Name";
            OperationTypeComboBox.SelectedValuePath = "Id";
        }


        /// <summary>
        /// Событие на кнопке - Сохранить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (OperationTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(HoursTextBox.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(MinutesTextBox.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(SecondsTextBox.Text))
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                OperationTypeEntity? operationType = OperationTypeComboBox.SelectedItem as OperationTypeEntity
                    ?? throw new Exception("Не удалось получить тип операции (SaveButton_Click)");
                int h = int.Parse(HoursTextBox.Text);
                int m = int.Parse(MinutesTextBox.Text);
                int s = int.Parse(SecondsTextBox.Text);
                TimeSpan standardTime = new (h, m, s);
                await ServiceOperation.Create(SelectedDetail.Id, operationType.Id, standardTime);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие на кнопке - Отмена
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
