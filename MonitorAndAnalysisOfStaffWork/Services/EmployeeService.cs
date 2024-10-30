using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;

namespace MonitorAndAnalysisOfStaffWork.Services
{
    /// <summary>
    /// Сервис по управлению сотрудниками
    /// </summary>
    public class EmployeeService
    {
        /// <summary>
        /// Контекст подключение к БД
        /// </summary>
        private readonly AppDbContext ContextApp;

        /// <summary>
        /// Сервис по управлению сотрудниками
        /// </summary>
        /// <exception cref="Exception"></exception>
        public EmployeeService()
        {
            ContextApp = App.AppHost.Services.GetService<AppDbContext>()
                ?? throw new Exception($"Ошибка при попытки получить контекст БД: {nameof(AppDbContext)}");

            ContextApp.ChangeTracker.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Получить сотрудника по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<EmployeeEntity> One(int id)
        {
            EmployeeEntity? employee = await ContextApp.Employees.FirstOrDefaultAsync(x => x.Id == id);
            return employee ?? throw new Exception($"Сотрудник с идентификатором {id} не найден");
        }


        /// <summary>
        /// Получить всех сотрудников
        /// </summary>
        /// <returns></returns>
        public async Task<List<EmployeeEntity>> List()
        {
            return await ContextApp.Employees.ToListAsync();
        }

        /// <summary>
        /// Получить всех сотрудников по табельному номеру сотрудника
        /// </summary>
        /// <param name="search">Табельный номер сотрудника</param>
        /// <returns></returns>
        public async Task<List<EmployeeEntity>> ListByNumber(string search)
        {
            return await ContextApp.Employees
                .Where(x => x.Number.ToLower().Contains(search.ToLower()) || x.FullName.ToLower().Contains(search.ToLower()))
                .ToListAsync();
        }


        /// <summary>
        /// Добавить сотрудника
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="position"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<EmployeeEntity> Create(string fullName, string position, string number)
        {
            EmployeeEntity employee = new()
            {
                FullName = fullName,
                Position = position,
                Number = number,
            };
            await ContextApp.Employees.AddAsync(employee);
            await ContextApp.SaveChangesAsync();
            return employee;
        }


        /// <summary>
        /// Обновить сотрудника
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fullName"></param>
        /// <param name="position"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<EmployeeEntity> Update(int id, string fullName, string position, string number)
        {
            EmployeeEntity employee = await One(id);
            employee.FullName = fullName;
            employee.Position = position;
            employee.Number = number;
            await ContextApp.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> Delete(int id)
        {
            EmployeeEntity employee = await One(id);
            ContextApp.RemoveRange(await ContextApp.Employees.Where(x => x.Id == id).ToListAsync());
            await ContextApp.SaveChangesAsync();
            return true;
        }
    }
}
