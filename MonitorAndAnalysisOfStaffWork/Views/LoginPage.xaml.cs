using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        /// <summary>
        /// Сервис по аутентификации
        /// </summary>
        private readonly AuthenticationService ServiceAuthentication;


        public LoginPage()
        {
            InitializeComponent();
            ServiceAuthentication = App.AppHost?.Services.GetService<AuthenticationService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(AuthenticationService)}");
            PopulateUsernames();
        }

        private async void PopulateUsernames()
        {
            try
            {
                var usernames = await ServiceAuthentication.GetAllUsernames();
                UsernameTextBox.ItemsSource = usernames;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка загрузки пользователей", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Login();
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрыть приложение
            Application.Current.Shutdown();

        }

        private async void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            // Проверяем, была ли нажата клавиша Enter
            if (e.Key == Key.Enter)
            {
                await Login();
            }
        }

        private async Task Login()
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            UserEntity user = await ServiceAuthentication.Login(username, password);

            // Сохранение авторизованного пользователя
            UserSession.CurrentUser = user;

            NavigationService?.Navigate(new ManufacturedDetailManagementPage());
        }
    }
}
