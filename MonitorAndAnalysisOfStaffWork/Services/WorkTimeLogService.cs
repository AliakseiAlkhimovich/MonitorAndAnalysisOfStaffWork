using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;

namespace MonitorAndAnalysisOfStaffWork.Services
{
    /// <summary>
    /// Сервис по управлению рабочем временем сотрудника
    /// </summary>
    public class WorkTimeLogService
    {
        /// <summary>
        /// Контекст подключение к БД
        /// </summary>
        private readonly AppDbContext ContextApp;

        /// <summary>
        /// Сервис по управлению рабочем временем сотрудника
        /// </summary>
        /// <exception cref="Exception"></exception>
        public WorkTimeLogService()
        {
            ContextApp = App.AppHost.Services.GetService<AppDbContext>()
                ?? throw new Exception($"Ошибка при попытки получить контекст БД: {nameof(AppDbContext)}");

            ContextApp.ChangeTracker.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Получить список рабочего времени сотрудника по сотруднику
        /// </summary>
        /// <param name="employeeId">Идентификатор сотрудника</param>
        /// <returns></returns>
        public async Task<List<WorkTimeLogEntity>> ListByEmployee(int employeeId)
        {
            return await ContextApp.WorkTimeLogs.Where(x => x.EmployeeId == employeeId).ToListAsync();
        }

        /// <summary>
        /// Добавить рабочего время сотруднику
        /// </summary>
        /// <param name="date">Дата</param>
        /// <param name="hoursWorked">Время</param>
        /// <param name="employeeId">Идентификатор сотрудника</param>
        /// <returns></returns>
        public async Task<WorkTimeLogEntity> Create(DateTime date, TimeSpan hoursWorked, int employeeId)
        {
            WorkTimeLogEntity workTimeLog = new()
            {
                Date = date,
                HoursWorked = hoursWorked,
                EmployeeId = employeeId
            };
            await ContextApp.WorkTimeLogs.AddAsync(workTimeLog);
            await ContextApp.SaveChangesAsync();
            return workTimeLog;
        }

        /// <summary>
        /// Удалить рабочее время у сотрудника
        /// </summary>
        /// <param name="id">Идентификатор записи рабочего времени</param>
        /// <returns></returns>
        public async Task<bool> Delete(int id)
        {
            ContextApp.RemoveRange(await ContextApp.WorkTimeLogs.Where(x => x.Id == id).ToListAsync());
            await ContextApp.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Получить список рабочего времени сотрудника за указанный год
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<WorkTimeLogEntity>> ListByYear(int year)
        {
            return await ContextApp.WorkTimeLogs
                .Include(x => x.Employee)
                .Where(x => x.Date.Year == year)
                .ToListAsync();
        }
    }
}
