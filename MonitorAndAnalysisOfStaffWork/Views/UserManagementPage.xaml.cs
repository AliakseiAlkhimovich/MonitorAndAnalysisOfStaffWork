using MonitorAndAnalysisOfStaffWork.Entities;
using System.Windows;
using System.Windows.Controls;
using MonitorAndAnalysisOfStaffWork.Services;
using Microsoft.Extensions.DependencyInjection;


namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Логика взаимодействия для UserManagementPage.xaml
    /// </summary>
    public partial class UserManagementPage : Page
    {
        /// <summary>
        /// Сервис по аутентификации
        /// </summary>
        private readonly UserService ServiceUser;

        public UserManagementPage()
        {
            InitializeComponent();
            ServiceUser = App.AppHost?.Services.GetService<UserService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(UserService)}");

            Load();
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        private async void Load()
        {
            List<UserEntity> users = await ServiceUser.List();
            UserDataGrid.ItemsSource = users;
        }

        private void CreateUser_Click(object sender, RoutedEventArgs e)
        {
            UserCreateAndUpdateWindow window = new();
            window.ShowDialog();
            Load();
        }

        /// <summary>
        /// Кнопка для просмотра информации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewDataGridButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button) return;
            if (button.DataContext is not UserEntity selectedUser) return;

            try
            {
                UserCreateAndUpdateWindow window = new(selectedUser);
                window.ShowDialog();
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DeleteDataGridButton_Click(object sender, RoutedEventArgs e)
        {

            if (sender is not Button button) return;
            if (button.DataContext is not UserEntity selectedUser) return;

            try
            {
                if (MessageBox.Show($"Удалить пользователя #{selectedUser.Id}: {selectedUser.Username}?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    await ServiceUser.Delete(selectedUser.Id);
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
