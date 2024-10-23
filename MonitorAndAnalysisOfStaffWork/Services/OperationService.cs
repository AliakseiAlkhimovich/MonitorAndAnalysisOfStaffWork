using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace MonitorAndAnalysisOfStaffWork.Services
{
    /// <summary>
    /// Сервис по управлению операциями
    /// </summary>
    public class OperationService
    {
        /// <summary>
        /// Контекст подключение к БД
        /// </summary>
        private readonly AppDbContext ContextApp;

        /// <summary>
        /// Сервис по управлению операциями
        /// </summary>
        /// <exception cref="Exception"></exception>
        public OperationService()
        {
            ContextApp = App.AppHost.Services.GetService<AppDbContext>()
                ?? throw new Exception($"Ошибка при попытки получить контекст БД: {nameof(AppDbContext)}");

            ContextApp.ChangeTracker.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Получить все типы операции
        /// </summary>
        /// <returns></returns>
        public async Task<List<OperationTypeEntity>> ListOperationType()
        {
            return await ContextApp.OperationTypes.ToListAsync();
        }

        /// <summary>
        /// Получить все типы операции
        /// </summary>
        /// <returns></returns>
        public async Task<List<OperationEntity>> ListByDetail(int detailId)
        {
            return await ContextApp.Operations
                .Include(x => x.Detail)
                .Include(x=>x.OperationType)
                .Where(x => x.DetailId == detailId)
                .ToListAsync();
        }

        /// <summary>
        /// Получить операцию по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<OperationEntity> One(int id)
        {
            OperationEntity? operation = await ContextApp.Operations
                .Include(x => x.Detail)
                .Include(x => x.OperationType)
                .FirstOrDefaultAsync(x => x.Id == id);
            return operation ?? throw new Exception($"Деталь с идентификатором {id} не найдена");
        }

        /// <summary>
        /// Получить все операции
        /// </summary>
        /// <returns></returns>
        public async Task<List<OperationEntity>> List()
        {
            return await ContextApp.Operations
                .Include(x => x.Detail)
                .Include(x => x.OperationType)
                .ToListAsync();
        }

        /// <summary>
        /// Добавить операцию
        /// </summary>
        /// <param name="detailId"></param>
        /// <param name="operationTypeId"></param>
        /// <param name="standardTime"></param>
        /// <returns></returns>
        public async Task<OperationEntity> Create(int detailId, int operationTypeId, TimeSpan standardTime)
        {
            OperationEntity operation = new()
            {
                StandardTime = standardTime,
                DetailId = detailId,
                OperationTypeId = operationTypeId,
            };
            await ContextApp.Operations.AddAsync(operation);
            await ContextApp.SaveChangesAsync();
            return operation;
        }

        /// <summary>
        /// Обновить операцию
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="detailId"></param>
        /// <param name="operationTypeId"></param>
        /// <param name="standardTime"></param>
        /// <returns></returns>
        public async Task<OperationEntity> Update(int id, int detailId, int operationTypeId, TimeSpan standardTime)
        {
            OperationEntity operation = await One(id);
            operation.StandardTime = standardTime;
            operation.DetailId = detailId;
            operation.OperationTypeId = operationTypeId;
            await ContextApp.SaveChangesAsync();
            return operation;
        }

        /// <summary>
        /// Удалить операцию
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        public async Task<bool> Delete(int id)
        {
            OperationEntity operation = await One(id);
            ContextApp.Operations.Remove(operation);
            await ContextApp.SaveChangesAsync();
            return true;
        }
    }
}
