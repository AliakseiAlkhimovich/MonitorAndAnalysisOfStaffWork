﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace MonitorAndAnalysisOfStaffWork.Views
{
    /// <summary>
    /// Окно добавления или редактирование изготовленной детали
    /// </summary>
    public partial class ManufacturedDetailCreateAndUpdatePage : Window
    {
        /// <summary>
        /// Выбранная изготовленная деталь
        /// </summary>
        private ManufacturedDetailEntity? UpdateManufacturedDetail { get; set; } = null;

        /// <summary>
        /// Результат поиска сотрудников по табельному номеру
        /// </summary>
        public ObservableCollection<EmployeeEntity> FilteredEmployees { get; set; } = [];

        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
        private EmployeeEntity? SelectedEmployee { get; set; } = null;

        /// <summary>
        /// Результат поиска деталей
        /// </summary>
        public ObservableCollection<DetailEntity> FilteredDetails { get; set; } = [];

        /// <summary>
        /// Выбранная деталь
        /// </summary>
        private DetailEntity? SelectedDetail { get; set; } = null;

        /// <summary>
        /// Сервис по управлению сотрудниками
        /// </summary>
        private readonly EmployeeService ServiceEmployee;

        /// <summary>
        /// Сервис по управлению деталями
        /// </summary>
        private readonly DetailService ServiceDetail;

        /// <summary>
        /// Сервис по управлению изготовленными деталями
        /// </summary>
        private readonly ManufacturedDetailService ServiceManufacturedDetail;


        /// <summary>
        /// Окно добавления или редактирование изготовленной детали
        /// </summary>
        /// <exception cref="Exception"></exception>
        public ManufacturedDetailCreateAndUpdatePage()
        {
            ServiceEmployee = App.AppHost.Services.GetService<EmployeeService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(EmployeeService)}");
            ServiceDetail = App.AppHost.Services.GetService<DetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(DetailService)}");
            ServiceManufacturedDetail = App.AppHost.Services.GetService<ManufacturedDetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(ManufacturedDetailService)}");
            InitializeComponent();
            DataContext = this;
            Load();
        }

        /// <summary>
        /// Окно добавления или редактирование изготовленной детали
        /// </summary>
        /// <param name="manufacturedDetail">Выбранная изготовленная деталь</param>
        /// <exception cref="Exception"></exception>
        public ManufacturedDetailCreateAndUpdatePage(ManufacturedDetailEntity manufacturedDetail)
        {
            ServiceEmployee = App.AppHost.Services.GetService<EmployeeService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(EmployeeService)}");
            ServiceDetail = App.AppHost.Services.GetService<DetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(DetailService)}");
            ServiceManufacturedDetail = App.AppHost.Services.GetService<ManufacturedDetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(ManufacturedDetailService)}");
            InitializeComponent();
            DataContext = this;
            UpdateManufacturedDetail = manufacturedDetail;
            SelectedEmployee = UpdateManufacturedDetail.Employee;
            SelectedDetail = UpdateManufacturedDetail.Detail;
            Load();
        }


        /// <summary>
        /// Загрузка данных
        /// </summary>
        private async void Load()
        {
            if (UpdateManufacturedDetail != null)
            {
                TitleTextBlock.Text = $"Редактирование детали {UpdateManufacturedDetail.DetailName}";
                ManufactureDateDatePicker.SelectedDate = UpdateManufacturedDetail.ManufactureDate;
                QuantityTextBox.Text = UpdateManufacturedDetail.Quantity.ToString();
            }

            if (SelectedEmployee != null)
            {
                SelectedEmployeeLabel.Content = SelectedEmployee.FullName;
            }
            else
            {
                SelectedEmployeeLabel.Content = "Сотрудник не выбран";
            }

            if (SelectedDetail != null)
            {
                SelectedDetailLabel.Content = $"{SelectedDetail.Number}.{SelectedDetail.Name} - {SelectedDetail.Designation}";
            }
            else
            {
                SelectedDetailLabel.Content = "Деталь не выбрана";
            }
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
        /// Обработчик изменения текста в поисковом поле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void EmployeeSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = EmployeeSearchBox.Text;

            // Поиск начинается при вводе 3 и более символов
            if (searchText.Length >= 3)
            {
                List<EmployeeEntity> employees = await ServiceEmployee.ListByNumber(searchText);
                FilteredEmployees.Clear();
                foreach (EmployeeEntity employee in employees)
                {
                    FilteredEmployees.Add(employee);
                }
            }
            else
            {
                FilteredEmployees.Clear();
            }
        }


        /// <summary>
        /// Событие на выбор сотрудника из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="Exception"></exception>
        private void EmployeeListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Проверяем, что выбранный элемент не null и является EmployeeEntity
            if (EmployeeListView.SelectedItem is EmployeeEntity employee)
            {
                SelectedEmployee = employee;
                SelectedEmployeeLabel.Content = SelectedEmployee.FullName;
            }
    else
            {
                // Если ничего не выбрано или выбранный элемент не EmployeeEntity
                SelectedEmployee = null; // Сбрасываем выбранного сотрудника
                SelectedEmployeeLabel.Content = "Выберите сотрудника"; // Уведомление пользователю
            }
        }


        /// <summary>
        /// Обработчик изменения текста в поисковом поле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DetailSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = DetailSearchBox.Text;
                if (searchText.Length >= 3)
                {
                    List<DetailEntity> details = await ServiceDetail.ListBySearch(searchText);
                    FilteredDetails.Clear();
                    foreach (DetailEntity detail in details)
                    {
                        FilteredDetails.Add(detail);
                    }
                }
                else
                {
                    FilteredDetails.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Событие на выбор детали из списка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Пробуем привести SelectedItem к типу DetailEntity
            DetailEntity? detail = DetailListView.SelectedItem as DetailEntity;

            // Проверяем, успешно ли привели к типу и не равен ли detail null
            if (detail != null)
            {
                SelectedDetail = detail;
                SelectedDetailLabel.Content = $"{SelectedDetail.Number}.{SelectedDetail.Name} - {SelectedDetail.Designation}";
            }
            else
            {
                // Если detail равно null, сбрасываем выбранную деталь и обновляем интерфейс
                SelectedDetail = null;
                SelectedDetailLabel.Content = "Выберите деталь"; // Уведомление пользователю }
            }
        }

        /// <summary>
        /// Событие по кнопке - Сохранить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void SaveButton_Click(object sender, System.EventArgs e)
        {
            if (SelectedEmployee == null)
            {
                MessageBox.Show("Сотрудник не выбран", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedDetail == null)
            {
                MessageBox.Show("Деталь не выбрана", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ManufactureDateDatePicker.SelectedDate == null)
            {

                MessageBox.Show("Дата изготовления деталей не задана", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DateTime manufactureDate = ManufactureDateDatePicker.SelectedDate.Value;
            if (string.IsNullOrEmpty(QuantityTextBox.Text) || int.TryParse(QuantityTextBox.Text, out int quantity) == false)
            {
                MessageBox.Show("Количество изготовленных деталей не задано или число некорректно", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                if (UpdateManufacturedDetail == null)
                {
                    ManufacturedDetailEntity manufacturedDetail = await ServiceManufacturedDetail.Create(SelectedEmployee.Id, SelectedDetail.Id, manufactureDate, quantity);
                }
                else
                {
                    ManufacturedDetailEntity manufacturedDetail = await ServiceManufacturedDetail.Update(UpdateManufacturedDetail.Id, SelectedEmployee.Id, SelectedDetail.Id, manufactureDate, quantity);
                }
                
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
        public void CancelButton_Click(object sender, System.EventArgs e)
        {
            Close();
        }
    }
}
