using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System.Windows;
using System.Windows.Controls;

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
            ServiceAuthentication = App.AppHost.Services.GetService<AuthenticationService>() 
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(AuthenticationService)}");
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;
                UserEntity user = await ServiceAuthentication.Login(username, password);

                // Сохранение авторизованного пользователя
                UserSession.CurrentUser = user;

                NavigationService?.Navigate(new ManufacturedDetailManagementPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                //NavigationService?.Navigate(new ManufacturedDetailManagementPage());
            }
            
            
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрыть приложение
            Application.Current.Shutdown();

        }

    }
}
