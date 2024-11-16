using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MonitorAndAnalysisOfStaffWork.Entities;
using System.Security.Cryptography;
using System.Text;

namespace MonitorAndAnalysisOfStaffWork.Services
{
    /// <summary>
    /// Сервис по аутентификации
    /// </summary>
    public class AuthenticationService
    {
        /// <summary>
        /// Контекст подключение к БД
        /// </summary>
        private readonly AppDbContext ContextApp;

        /// <summary>
        /// Сервис по аутентификации
        /// </summary>
        public AuthenticationService()
        {
            ContextApp = App.AppHost.Services.GetService<AppDbContext>()
                ?? throw new Exception($"Ошибка при попытки получить контекст БД: {nameof(AppDbContext)}");
        }

        /// <summary>
        /// Получить ХЭШ пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>ХЭШ пароля</returns>
        public static string GetHashPasswod(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UserEntity> Login(string username, string password)
        {
            UserEntity? user = await ContextApp.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                throw new Exception($"Не удалось найти учетную запись с этим именем пользователя: {username}");
            }
            string passwordHash = GetHashPasswod(password);
            if (user.PasswordHash != passwordHash)
            {
                throw new Exception($"Введен не верный пароль");
            }
            return user;
        }



        /// <summary>
        /// Получить все роли
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleEntity>> GetRolesAsync()
        {
            return await ContextApp.Roles.ToListAsync();
        }


        /// <summary>
        /// Получить все роли
        /// </summary>
        /// <returns></returns>
        public List<RoleEntity> GetRoles()
        {
            return ContextApp.Roles.AsNoTracking().ToList();
        }
 

    }
}
