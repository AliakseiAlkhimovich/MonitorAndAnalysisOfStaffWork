using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;

namespace MonitorAndAnalysisOfStaffWork.Services
{

    /// <summary>
    /// Сервис по управлению деталями
    /// </summary>
    public class DetailService
    {
        /// <summary>
        /// Контекст подключение к БД
        /// </summary>
        private readonly AppDbContext ContextApp;


        /// <summary>
        /// Сервис по управлению деталями
        /// </summary>
        /// <exception cref="Exception"></exception>
        public DetailService()
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
        public async Task<DetailEntity> One(int id)
        {
            DetailEntity? detail = await ContextApp.Details.FirstOrDefaultAsync(x => x.Id == id);
            return detail ?? throw new Exception($"Деталь с идентификатором {id} не найдена");
        }

        /// <summary>
        /// Получить все детали
        /// </summary>
        /// <returns></returns>
        public async Task<List<DetailEntity>> List()
        {
            return await ContextApp.Details.Include(x => x.Operations).ThenInclude(o => o.OperationType).ToListAsync();
        }

        /// <summary>
        /// Получить все детали по наименованию, обозначению или номеру
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <returns></returns>
        public async Task<List<DetailEntity>> ListBySearch(string search)
        {
            search = search.ToLower();
            return await ContextApp.Details
                .Where(x => x.Name.ToLower().Contains(search) || x.Designation.ToLower().Contains(search) || x.Number.ToLower().Contains(search))
                .ToListAsync();
        }

        /// <summary>
        /// Добавить деталь
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <param name="designation"></param>
        /// <returns></returns>
        public async Task<DetailEntity> Create(string name, string number, string designation)
        {
            DetailEntity detail = new()
            {
                Name = name,
                Number = number,
                Designation = designation
            };
            await ContextApp.Details.AddAsync(detail);
            await ContextApp.SaveChangesAsync();
            return detail;
        }

        /// <summary>
        /// Обновить деталь
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <param name="designation"></param>
        /// <returns></returns>
        public async Task<DetailEntity> Update(int id, string name, string number, string designation)
        {
            DetailEntity detail = await One(id);
            detail.Name = name;
            detail.Number = number;
            detail.Designation = designation;
            await ContextApp.SaveChangesAsync();
            return detail;
        }

        public async Task<bool> Delete(int id)
        {
            DetailEntity employee = await One(id);
            ContextApp.RemoveRange(await ContextApp.Details.Where(x => x.Id == id).ToListAsync());
            await ContextApp.SaveChangesAsync();
            return true;
        }
    }

}
