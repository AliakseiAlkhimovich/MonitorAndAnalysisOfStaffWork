using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;

namespace MonitorAndAnalysisOfStaffWork.Services
{
    /// <summary>
    /// Сервис по управлению учетными записями
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// Контекст подключение к БД
        /// </summary>
        private readonly AppDbContext ContextApp;

        /// <summary>
        /// Сервис по управлению учетными записями
        /// </summary>
        /// <exception cref="Exception"></exception>
        public UserService()
        {
            ContextApp = App.AppHost?.Services.GetService<AppDbContext>()
                ?? throw new Exception($"Ошибка при попытки получить контекст БД: {nameof(AppDbContext)}");

            ContextApp.ChangeTracker.LazyLoadingEnabled = false;
        }

        /// <summary>
        /// Получить учетную запись по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<UserEntity> One(int id)
        {
            UserEntity? user = await ContextApp.Users.Include(x=>x.Role).FirstOrDefaultAsync(x=>x.Id == id);
            return user ?? throw new Exception($"Пользователь с идентификатором {id} не найден");
        }

        /// <summary>
        /// Получить все учетные записи
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserEntity>> List()
        {
            return await ContextApp.Users.Include(x => x.Role).ToListAsync();
        }


        /// <summary>
        /// Создать новую учетную запись
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="fullName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<UserEntity> Create(string username, string password, string fullName, int roleId)
        {
            UserEntity user = new()
            {
                Username = username,
                PasswordHash = AuthenticationService.GetHashPasswod(password),
                FullName = fullName,
                RoleId = roleId
            };
            await ContextApp.AddAsync(user);
            await ContextApp.SaveChangesAsync();
            return user;
        }


        /// <summary>
        /// Обновить учетную запись
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="fullName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<UserEntity> Update(int id, string username, string password, string fullName, int roleId)
        {
            UserEntity user = await One(id);
            user.Username = username;
            user.PasswordHash = AuthenticationService.GetHashPasswod(password);
            user.FullName = fullName;
            user.RoleId = roleId;
            await ContextApp.SaveChangesAsync();
            return user;
        }


        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> Delete(int id)
        {
            UserEntity? user = await One(id);

            if (await ContextApp.Users.Where(x => x.RoleId == 1).CountAsync() == 1) {

                if (user.RoleId == 1)
                {
                    throw new Exception($"Удаление запрещено. В системе необходимо наличие хотя бы одной учетной записи с ролью Администратор");
                }
            }

            ContextApp.RemoveRange(await ContextApp.Users.Where(x => x.Id == id).ToListAsync());
            await ContextApp.SaveChangesAsync();
            return true;
        }
    }
}
