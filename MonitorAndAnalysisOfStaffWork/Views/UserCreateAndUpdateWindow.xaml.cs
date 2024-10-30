using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Окно создания и редактирования учетной записи
    /// </summary>
    public partial class UserCreateAndUpdateWindow : Window
    {
        /// <summary>
        /// Учетная запись для обновления
        /// </summary>
        private UserEntity? UpdateUser { get; set; } = null;

        /// <summary>
        /// Сервис по аутентификации
        /// </summary>
        private readonly UserService ServiceUser;

        /// <summary>
        ///  Сервис по аутентификации
        /// </summary>
        private readonly AuthenticationService ServiceAuthentication;


        /// <summary>
        /// Окно создания и редактирования учетной записи
        /// </summary>
        /// <param name="context"></param>
        public UserCreateAndUpdateWindow()
        {
            InitializeComponent();
            ServiceUser = App.AppHost.Services.GetService<UserService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(UserService)}");
            ServiceAuthentication = App.AppHost.Services.GetService<AuthenticationService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(AuthenticationService)}");
            Load();
        }


        /// <summary>
        /// Окно создания и редактирования учетной записи
        /// </summary>
        /// <param name="context"></param>
        public UserCreateAndUpdateWindow(UserEntity updateUser)
        {
            InitializeComponent();
            ServiceUser = App.AppHost.Services.GetService<UserService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(UserService)}");
            ServiceAuthentication = App.AppHost.Services.GetService<AuthenticationService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(AuthenticationService)}");
            UpdateUser = updateUser;
            TitleWindow.Text = $"Обновление учетной записи {UpdateUser.Username}";
            Load();
        }

        /// <summary>
        /// Загрузка данных
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Создать или обновить учетную запись
        /// </summary>
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(UsernameTextBox.Text) || PasswordBox.Password == string.Empty || PasswordRepeatBox.Password == string.Empty || FullNameTextBox.Text == string.Empty || RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PasswordBox.Password != PasswordRepeatBox.Password)
            {
                MessageBox.Show("Введеные пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }
            try
            {
                if (UpdateUser == null)
                {
                    UserEntity user = await ServiceUser.Create(UsernameTextBox.Text, PasswordBox.Password, FullNameTextBox.Text, (int)RoleComboBox.SelectedValue);
                }
                else
                {
                    UserEntity user = await ServiceUser.Update(UpdateUser.Id, UsernameTextBox.Text, PasswordBox.Password, FullNameTextBox.Text, (int)RoleComboBox.SelectedValue);
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
