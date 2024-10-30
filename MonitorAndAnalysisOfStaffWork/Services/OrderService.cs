using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace MonitorAndAnalysisOfStaffWork.Services
{
    /// <summary>
    /// Сервис по генерации и создание отчетов
    /// </summary>
    public class OrderService
    {
        /// <summary>
        /// Сервис по управлению деталями
        /// </summary>
        private readonly WorkTimeLogService ServiceWorkTimeLog;

        /// <summary>
        /// Сервис по управлению деталями
        /// </summary>
        private readonly DetailService ServiceDetail;

        /// <summary>
        /// Сервис по управлению деталями
        /// </summary>
        private readonly ManufacturedDetailService ServiceManufacturedDetail;

        /// <summary>
        /// Сервис по генерации и создание отчетов
        /// </summary>
        /// <exception cref="Exception"></exception>
        public OrderService()
        {
            ServiceWorkTimeLog = App.AppHost.Services.GetService<WorkTimeLogService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(WorkTimeLogService)}");
            ServiceDetail = App.AppHost.Services.GetService<DetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(DetailService)}");
            ServiceManufacturedDetail = App.AppHost.Services.GetService<ManufacturedDetailService>()
                ?? throw new Exception($"Ошибка при инициализация сервиса: {nameof(ManufacturedDetailService)}");

        }


        /// <summary>
        /// Сформировать отчет - Табель учета рабочего времени работников
        /// </summary>
        /// <param name="path"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task WorkTimeLog(string path, int year)
        {
            List<WorkTimeLogEntity> workTimeLogs = await ServiceWorkTimeLog.ListByYear(year);

            // Группируем рабочие часы по сотруднику и месяцу
            var employeeLogs = workTimeLogs
                .GroupBy(x => new { x.Employee, x.Date.Month })
                .Select(g => new
                {
                    Employee = g.Key.Employee,
                    Month = g.Key.Month,
                    TotalHours = g.Sum(x => x.HoursWorked.TotalHours)
                })
                .GroupBy(x => x.Employee)
                .ToList();



            using (ExcelPackage package = new())
            {
                var sheet = package.Workbook.Worksheets.Add("Отчет");
                sheet.Cells[1, 1].Value = "";

                // Заголовок
                sheet.Cells[1, 1].Value = "Табель учета рабочего времени работников";
                sheet.Cells[2, 1].Value = $"За {year} год";

                // Стили заголовка
                sheet.Cells[1, 1, 1, 16].Merge = true;
                sheet.Cells[1, 1].Style.Font.Bold = true;
                sheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[2, 1, 2, 16].Merge = true;
                sheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Заголовки столбцов
                sheet.Cells[4, 1].Value = "ФИО";
                sheet.Cells[4, 2].Value = "Должность";
                sheet.Cells[4, 3].Value = "Табельный номер";


                // Генерируем месяцы
                for (int i = 1; i <= 12; i++)
                {
                    sheet.Cells[4, i + 3].Value = new DateTime(year, i, 1).ToString("MMMM");
                }
                sheet.Cells[4, 16].Value = "Итого часов за год";

                int row = 5; // Начиная с 5 строки заполняем данные о сотрудниках

                // Перебираем сотрудников и записываем их данные
                foreach (var employeeLogGroup in employeeLogs)
                {
                    var employee = employeeLogGroup.Key;
                    sheet.Cells[row, 1].Value = employee.FullName;
                    sheet.Cells[row, 2].Value = employee.Position;
                    sheet.Cells[row, 3].Value = employee.Number;

                    double yearlyTotal = 0; // Общие часы за год

                    // Перебираем данные по месяцам и заполняем отработанные часы
                    for (int month = 1; month <= 12; month++)
                    {
                        var logForMonth = employeeLogGroup.FirstOrDefault(x => x.Month == month);
                        double hoursWorked = logForMonth?.TotalHours ?? 0;
                        sheet.Cells[row, month + 3].Value = hoursWorked;
                        yearlyTotal += hoursWorked;
                    }

                    // Записываем общее количество отработанных часов за год
                    sheet.Cells[row, 16].Value = yearlyTotal;
                    row++;
                }

                byte[] byteExcelData = package.GetAsByteArray();
                using (FileStream fileStream = new(path, FileMode.Create))
                {
                    await fileStream.WriteAsync(byteExcelData);
                }
            }
        }

        /// <summary>
        /// Сформировать отчет - Список деталей с нормами времени по операциям
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task DetailAndOperation(string path)
        {
            // Получаем список всех деталей с их операциями

            List<DetailEntity> details = await ServiceDetail.List();

            // Создаем Excel файл с EPPlus
            using (ExcelPackage package = new())
            {
                var sheet = package.Workbook.Worksheets.Add("Отчет по операциям");

                // Устанавливаем заголовок и подзаголовок
                sheet.Cells[1, 1].Value = "Отчет по операциям изготовления деталей";
                sheet.Cells[2, 1].Value = $"Дата формирования: {DateTime.Now:dd.MM.yyyy}";


                // Стили заголовка
                sheet.Cells[1, 1, 1, 5].Merge = true;
                sheet.Cells[1, 1].Style.Font.Bold = true;
                sheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[2, 1, 2, 5].Merge = true;
                sheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Заголовки столбцов
                sheet.Cells[4, 1].Value = "ФИО";
                sheet.Cells[4, 2].Value = "Должность";
                sheet.Cells[4, 3].Value = "Табельный номер";



                // Устанавливаем заголовки для столбцов
                sheet.Cells[4, 1].Value = "Наименование детали";
                sheet.Cells[4, 2].Value = "Номер детали (артикул)";
                sheet.Cells[4, 3].Value = "Обозначение";
                sheet.Cells[4, 4].Value = "Тип операции";
                sheet.Cells[4, 5].Value = "Стандартное время";

                int row = 5; // Строка для начала записи деталей и операций

                // Перебираем каждую деталь и её операции
                foreach (var detail in details)
                {
                    // Записываем общие данные по детали
                    sheet.Cells[row, 1].Value = detail.Name;
                    sheet.Cells[row, 2].Value = detail.Number;
                    sheet.Cells[row, 3].Value = detail.Designation;

                    // Перебираем операции для данной детали
                    foreach (var operation in detail.Operations)
                    {
                        sheet.Cells[row, 4].Value = operation.OperationTypeName; // Название типа операции (например, токарная)
                        sheet.Cells[row, 5].Value = operation.StandardTimeString; // Стандартное время выполнения операции
                        row++;
                    }

                    // Добавляем пустую строку между деталями для удобочитаемости
                    row++;
                }

                byte[] byteExcelData = package.GetAsByteArray();
                using (FileStream fileStream = new(path, FileMode.Create))
                {
                    await fileStream.WriteAsync(byteExcelData);
                }
            }
        }


        /// <summary>
        /// Сформировать отчет - Список деталей изготовленных работниками за период времени
        /// </summary>
        /// <param name="path"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task ManufacturedDetail(string path, DateTime startDate, DateTime endDate)
        {
            // Получаем данные о произведенных деталях за указанный период
            List<ManufacturedDetailEntity> manufacturedDetails = await ServiceManufacturedDetail.ListByPeriod(startDate, endDate);

            // Создаем новый Excel пакет
            using (ExcelPackage package = new())
            {
                var sheet = package.Workbook.Worksheets.Add("Отчет");

                // Заголовок и подзаголовок
                sheet.Cells[1, 1].Value = "Отчет о произведенных деталях";
                sheet.Cells[2, 1].Value = $"За период: {startDate.ToString("dd.MM.yyyy")} - {endDate.ToString("dd.MM.yyyy")}";

                // Стили заголовка
                sheet.Cells[1, 1, 1, 5].Merge = true;
                sheet.Cells[1, 1].Style.Font.Bold = true;
                sheet.Cells[1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[2, 1, 2, 5].Merge = true;
                sheet.Cells[2, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Заголовок таблицы
                sheet.Cells[4, 1].Value = "ФИО сотрудника";
                sheet.Cells[4, 2].Value = "Должность";
                sheet.Cells[4, 3].Value = "Табельный номер";
                sheet.Cells[4, 4].Value = "Номер детали";
                sheet.Cells[4, 5].Value = "Название детали";
                sheet.Cells[4, 6].Value = "Количество";
                sheet.Cells[4, 7].Value = "Дата изготовления";

                // Стили заголовка таблицы
                sheet.Cells[4, 1, 4, 7].Style.Font.Bold = true;
                sheet.Cells[4, 1, 4, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells[4, 1, 4, 7].Style.Border.BorderAround(ExcelBorderStyle.Thin);

                // Заполняем строки данными
                int row = 5;
                foreach (var detail in manufacturedDetails)
                {
                    sheet.Cells[row, 1].Value = detail.Employee.FullName;
                    sheet.Cells[row, 2].Value = detail.Employee.Position;
                    sheet.Cells[row, 3].Value = detail.Employee.Number;
                    sheet.Cells[row, 4].Value = detail.Detail.Number;
                    sheet.Cells[row, 5].Value = detail.Detail.Name;
                    sheet.Cells[row, 6].Value = detail.Quantity;
                    sheet.Cells[row, 7].Value = detail.ManufactureDate.ToString("dd.MM.yyyy");

                    row++;
                }

                // Автоширина для колонок
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

                // Получаем Excel данные в виде массива байтов
                byte[] byteExcelData = package.GetAsByteArray();

                // Сохраняем файл
                using (FileStream fileStream = new(path, FileMode.Create))
                {
                    await fileStream.WriteAsync(byteExcelData);
                }
            }
        }
    }
}
