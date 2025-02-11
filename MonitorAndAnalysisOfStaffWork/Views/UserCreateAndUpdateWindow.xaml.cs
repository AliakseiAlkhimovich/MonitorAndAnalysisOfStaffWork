using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System.Windows;
using System.Windows.Controls;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Окно создания и редактирования учетной записи
    /// </summary>
    public partial class UserCreateAndUpdateWindow : Window
    {
        private UserEntity? UpdateUser { get; set; } = null;
        private readonly UserService ServiceUser;
        private readonly AuthenticationService ServiceAuthentication;

        public UserCreateAndUpdateWindow()
        {
            InitializeComponent();
            ServiceUser = App.AppHost?.Services.GetService<UserService>()
                ?? throw new Exception($"Ошибка при инициализации сервиса: {nameof(UserService)}");
            ServiceAuthentication = App.AppHost.Services.GetService<AuthenticationService>()
                ?? throw new Exception($"Ошибка при инициализации сервиса: {nameof(AuthenticationService)}");
            Load();
        }

        public UserCreateAndUpdateWindow(UserEntity updateUser)
        {
            InitializeComponent();
            ServiceUser = App.AppHost?.Services.GetService<UserService>()
                ?? throw new Exception($"Ошибка при инициализации сервиса: {nameof(UserService)}");
            ServiceAuthentication = App.AppHost.Services.GetService<AuthenticationService>()
                ?? throw new Exception($"Ошибка при инициализации сервиса: {nameof(AuthenticationService)}");
            UpdateUser = updateUser;
            TitleWindow.Text = $"Обновление учетной записи {UpdateUser.Username}";
            Load();
        }

        private void Load()
        {
            List<RoleEntity> roles = ServiceAuthentication.GetRoles();
            RoleComboBox.ItemsSource = roles;
            RoleComboBox.DisplayMemberPath = "Title";
            RoleComboBox.SelectedValuePath = "Id";
            if (UpdateUser != null)
            {
                UsernameTextBox.Text = UpdateUser.Username;
                FullNameTextBox.Text = UpdateUser.FullName;
                RoleComboBox.SelectedValue = UpdateUser.RoleId;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordWarningTextBlock.Visibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(UsernameTextBox.Text) || string.IsNullOrEmpty(FullNameTextBox.Text) || RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка длины пароля
            if (PasswordBox.Password.Length < 6)
            {
                PasswordWarningTextBlock.Text = "Пароль должен содержать не менее 6 символов.";
                PasswordWarningTextBlock.Visibility = Visibility.Visible;
                return;
            }

            if (PasswordBox.Password != PasswordRepeatBox.Password)
            {
                MessageBox.Show("Введенные пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (UpdateUser == null)
                {
                    await ServiceUser.Create(UsernameTextBox.Text, PasswordBox.Password, FullNameTextBox.Text, (int)RoleComboBox.SelectedValue);
                }
                else
                {
                    await ServiceUser.Update(UpdateUser.Id, UsernameTextBox.Text, PasswordBox.Password, FullNameTextBox.Text, (int)RoleComboBox.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePassword();
        }

        private void PasswordRepeatBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePassword();
        }

        private void ValidatePassword()
        {
            PasswordWarningTextBlock.Visibility = Visibility.Collapsed;

            if (PasswordBox.Password.Length < 6)
            {
                PasswordWarningTextBlock.Text = "Пароль должен содержать не менее 6 символов.";
                PasswordWarningTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
