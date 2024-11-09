using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Views;
using System.ComponentModel;
using System.Windows;

namespace MonitorAndAnalysisOfStaffWork
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateToolbar();
            MainFrame.Navigate(new LoginPage()); // При загрузке отображаем страницу авторизации

            // Подписка на событие изменения UserSession
            UserSession.StaticPropertyChanged += UserSession_PropertyChanged;
        }


        // Обработчик события изменения UserSession
        private void UserSession_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserSession.CurrentUser))
            {
                UserEntity? user = UserSession.CurrentUser;
                UpdateToolbar();
            }
        }
        private void UpdateToolbar()
        {
            UserEntity? user = UserSession.CurrentUser;
            if (user == null)
            {
                LoginButton.Visibility = Visibility.Visible;
                LoginButtoSeparatorn.Visibility = Visibility.Collapsed;

                UserManagementButton.Visibility = Visibility.Collapsed;
                UserManagementButtonSeparator.Visibility = Visibility.Collapsed;

                ManufacturedDetailManagementButton.Visibility = Visibility.Collapsed;
                ManufacturedDetailManagementButtonSeparator.Visibility = Visibility.Collapsed;

                EmployeeManagementButton.Visibility = Visibility.Collapsed;
                EmployeeManagementButtonSeparator.Visibility = Visibility.Collapsed;

                WorkTimeLogManagemenButton.Visibility = Visibility.Collapsed;
                WorkTimeLogManagemenButtonSeparator.Visibility = Visibility.Collapsed;

                DetailManagementButton.Visibility = Visibility.Collapsed;
                DetailManagementButtonSeparator.Visibility = Visibility.Collapsed;

                OperationManagementButton.Visibility = Visibility.Collapsed;
                OperationManagementButtonSeparator.Visibility = Visibility.Collapsed;


                OperationTypeManagementButton.Visibility = Visibility.Collapsed;
                OperationTypeManagementButtonSeparator.Visibility = Visibility.Collapsed;


                LoginTitleTextBlock.Text = "Пользователь не авторизован";
                LogoutButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                LoginButton.Visibility = Visibility.Collapsed;
                LoginButtoSeparatorn.Visibility = Visibility.Collapsed;
                if (user.RoleId == 1)
                {
                    UserManagementButton.Visibility = Visibility.Visible;
                    UserManagementButtonSeparator.Visibility = Visibility.Visible;
                }
                else
                {

                    UserManagementButton.Visibility = Visibility.Collapsed;
                    UserManagementButtonSeparator.Visibility = Visibility.Collapsed;
                }

                ManufacturedDetailManagementButton.Visibility = Visibility.Visible;
                ManufacturedDetailManagementButtonSeparator.Visibility = Visibility.Visible;

                EmployeeManagementButton.Visibility = Visibility.Visible;
                EmployeeManagementButtonSeparator.Visibility = Visibility.Visible;

                WorkTimeLogManagemenButton.Visibility = Visibility.Visible;
                WorkTimeLogManagemenButtonSeparator.Visibility = Visibility.Visible;

                DetailManagementButton.Visibility = Visibility.Visible;
                DetailManagementButtonSeparator.Visibility = Visibility.Visible;

                OperationManagementButton.Visibility = Visibility.Visible;
                OperationManagementButtonSeparator.Visibility = Visibility.Visible;


                OperationTypeManagementButton.Visibility = Visibility.Visible;
                OperationTypeManagementButtonSeparator.Visibility = Visibility.Visible;

                LoginTitleTextBlock.Text = user.FullName;
                LogoutButton.Visibility = Visibility.Visible;

            }
        }

        /// <summary>
        /// Событие по кнопке - Авторизация
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new LoginPage());
        }

        /// <summary>
        /// Открыть страницу "Учетные записи"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UserManagementPage());
        }
        /// <summary>
        /// Открыть страницу "Сотрудники"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ManufacturedDetailManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ManufacturedDetailManagementPage());
        }

        /// <summary>
        /// Открыть страницу "Сотрудники"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmployeeManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EmployeeManagementPage());
        }

        /// <summary>
        /// Открыть страницу "Сотрудники"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkTimeLogManagemenButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new WorkTimeLogManagementPage());
        }


        /// <summary>
        /// Открыть страницу "Изготовляемые детали"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DetailManagementPage());
        }


        /// <summary>
        /// Открыть страницу "Изготовляемые детали"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OperationManagementPage());
        }


        /// <summary>
        /// Открыть страницу "Изготовляемые детали"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperationTypeManagementButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OperationTypeManagementPage());
        }

        /// <summary>
        /// Событие на кнопке - Выход
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            UserSession.CurrentUser = null;
            MainFrame.Navigate(new LoginPage()); // При загрузке отображаем страницу авторизации

        }

        /// <summary>
        /// Событие на кнопке - О программе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutButton_Click(Object sender, RoutedEventArgs e)
        {
            List<string> texts = [];
            texts.Add("Программное средство для мониторинга и анализа эффективности труда персонала");
            texts.Add("\n");
            texts.Add("Функционал:");
            texts.Add("- Ведение табеля учёта рабочего времени сотрудников по месяцам с возможностью сохранения его в PDF или Excel файла, а также выводом его на печать (ФИО, должность, табельный номер сотрудника)");
            texts.Add("- Список деталей с нормой времени по операциям на их изготовление, например токарная операция, фрезерная операция и т.д. Должна быть возможность добавления операций в базу данных, а также добавление деталей в этот список и корректировки уже имеющихся. Возможность сохранения его в Excel файл.");
            texts.Add("- Список деталей (номер детали, название детали, количество сделанных деталей, дата изготовления), изготовленных работниками (ФИО, должность, табельный номер сотрудника) за период времени (месяц, квартал, год).\r\nВозможность добавления, удаления, корректировки, сохранения его в Excel файл.");
            texts.Add("\n");
            texts.Add("ФИО: Альхимович А.Л.");
            texts.Add("Номер группы: Пз2-22ПО");
            texts.Add("\n");
            texts.Add("\t\t\t\t\t\t2024");

            MessageBox.Show(string.Join("\n",texts),"О программе", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}