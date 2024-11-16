using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System.Windows;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Окно создания и редактирования детали
    /// </summary>
    public partial class DetailCreateAndUpdateWindow : Window
    {
        /// <summary>
        /// Обновляемая деталь
        /// </summary>
        private DetailEntity? UpdateDetail { get; set; } = null;

        /// <summary>
        /// Сервис по управлению деталями
        /// </summary>
        private readonly DetailService ServiceDetail;

        /// <summary>
        /// Окно создания и редактирования детали
        /// </summary>
        public DetailCreateAndUpdateWindow()
        {
            InitializeComponent();
            ServiceDetail = App.AppHost?.Services.GetService<DetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(DetailService)}");
        }
        /// <summary>
        /// Окно создания и редактирования детали
        /// </summary>
        public DetailCreateAndUpdateWindow(DetailEntity updateDetail)
        {
            InitializeComponent();
            ServiceDetail = App.AppHost?.Services.GetService<DetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(DetailService)}");
            UpdateDetail = updateDetail;

            TitleWindow.Text = $"Редактирование детали {updateDetail.Number}";

            NameTextBox.Text = UpdateDetail.Name;
            NumberTextBox.Text = UpdateDetail.Number;
            DesignationTextBox.Text = UpdateDetail.Designation;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(NameTextBox.Text))
            {
                MessageBox.Show("Заполните Наименование", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(NumberTextBox.Text))
            {
                MessageBox.Show("Заполните Номер детали (артикул):", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
          
            try
            {
                if (UpdateDetail == null)
                {
                    await ServiceDetail.Create(NameTextBox.Text, NumberTextBox.Text, DesignationTextBox.Text);
                }
                else
                {
                    await ServiceDetail.Update(UpdateDetail.Id, NameTextBox.Text, NumberTextBox.Text, DesignationTextBox.Text);
                }
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
