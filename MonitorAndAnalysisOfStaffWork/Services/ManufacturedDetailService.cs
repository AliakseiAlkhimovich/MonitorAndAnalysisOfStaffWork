using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;

namespace MonitorAndAnalysisOfStaffWork.Services
{
    /// <summary>
    /// Сервис по управлению изготовленными деталями
    /// </summary>
    public class ManufacturedDetailService
    {
        /// <summary>
        /// Контекст подключение к БД
        /// </summary>
        private readonly AppDbContext ContextApp;

        /// <summary>
        /// Сервис по управлению изготовленными деталями
        /// </summary>
        /// <exception cref="Exception"></exception>
        public ManufacturedDetailService()
        {
            ContextApp = App.AppHost?.Services.GetService<AppDbContext>()
                ?? throw new Exception($"Ошибка при попытки получить контекст БД: {nameof(AppDbContext)}");
            ContextApp.ChangeTracker.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Получить деталь по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ManufacturedDetailEntity> One(int id)
        {
            ManufacturedDetailEntity? manufacturedDetail = await ContextApp.ManufacturedDetails.FirstOrDefaultAsync(x => x.Id == id);
            return manufacturedDetail ?? throw new Exception($"Деталь с идентификатором {id} не найдена");
        }

        /// <summary>
        /// Получить все детали
        /// </summary>
        /// <returns></returns>
        public async Task<List<ManufacturedDetailEntity>> List()
        {
            return await ContextApp.ManufacturedDetails
                .Include(x => x.Employee)
                .Include(x => x.Detail)
                .ToListAsync();
        }

        /// <summary>
        /// Получить все детали по наименованию, обозначению или номеру
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns></returns>
        public async Task<List<ManufacturedDetailEntity>> ListBySearch(string search)
        {
            search = search.ToLower();
            List<int> detailIds = await ContextApp.Details
                .Where(x => x.Name.ToLower().Contains(search) || x.Designation.ToLower().Contains(search) || x.Number.ToLower().Contains(search))
                .Select(x => x.Id)
                .ToListAsync();
            return await ContextApp.ManufacturedDetails
                .Include(x => x.Employee)
                .Include(x => x.Detail)
                .Where(x => detailIds.Contains(x.DetailId))
                .ToListAsync();
        }

        /// <summary>
        /// Добавить деталь
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="detailId"></param>
        /// <param name="manufactureDate"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task<ManufacturedDetailEntity> Create(int employeeId, int detailId, DateTime manufactureDate, int quantity)
        {
            ManufacturedDetailEntity manufacturedDetail = new()
            {
                EmployeeId = employeeId,
                DetailId = detailId,
                ManufactureDate = manufactureDate,
                Quantity = quantity
            };
            await ContextApp.ManufacturedDetails.AddAsync(manufacturedDetail);
            await ContextApp.SaveChangesAsync();
            return manufacturedDetail;
        }

        /// <summary>
        /// Обновить деталь
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="detailId"></param>
        /// <param name="manufactureDate"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public async Task<ManufacturedDetailEntity> Update(int id, int employeeId, int detailId, DateTime manufactureDate, int quantity)
        {
            ManufacturedDetailEntity manufacturedDetail = await One(id);
            manufacturedDetail.EmployeeId = employeeId;
            manufacturedDetail.DetailId = detailId;
            manufacturedDetail.ManufactureDate = manufactureDate;
            manufacturedDetail.Quantity = quantity;
            await ContextApp.SaveChangesAsync();
            return manufacturedDetail;
        }

        /// <summary>
        /// Удалить деталь
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Delete(int id)
        {
            ManufacturedDetailEntity manufacturedDetail = await One(id);
            ContextApp.ManufacturedDetails.Remove(manufacturedDetail);
            await ContextApp.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Получить все детали за указанный период
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<ManufacturedDetailEntity>> ListByPeriod(DateTime startDate, DateTime endDate)
        {
            return await ContextApp.ManufacturedDetails
                .Include(x => x.Employee)
                .Include(x => x.Detail)
                .Where(x => x.ManufactureDate >= startDate && x.ManufactureDate <= endDate)
                .ToListAsync();
        }
    }
}
