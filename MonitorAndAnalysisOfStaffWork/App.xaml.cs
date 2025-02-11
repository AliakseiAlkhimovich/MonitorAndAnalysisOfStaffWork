using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Hosting;
using MonitorAndAnalysisOfStaffWork.Services;


namespace MonitorAndAnalysisOfStaffWork
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public IConfiguration Configuration { get; }

        public App()
        {
            // Настройка конфигурации
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Имя строки подключения
            string connectionStringName = "MSQLConnection";

            // Проверка на существование строки подключения
            var connectionString = Configuration.GetConnectionString(connectionStringName);
            if (string.IsNullOrEmpty(connectionString))
            {
                MessageBox.Show($"Ошибка: Строка подключения '{connectionStringName}' не найдена в файле appsettings.json.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(); // Завершение приложения
                return; // Выход из метода
            }

            // Создание и настройка хоста
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Регистрация DbContext с подключением к БД
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(connectionString));

                    services.AddScoped<AuthenticationService>();
                    services.AddScoped<UserService>();
                    services.AddScoped<EmployeeService>();
                    services.AddScoped<WorkTimeLogService>();
                    services.AddScoped<OrderService>();
                    services.AddScoped<DetailService>();
                    services.AddScoped<OperationService>();
                    services.AddScoped<ManufacturedDetailService>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();

            // Запуск хоста
            AppHost.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    MessageBox.Show("Ошибка при запуске приложения: " + task.Exception?.InnerException?.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown(); // Завершение приложения
                }
            });

            // Запуск главного окна через сервисы
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>()!;

            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            // Остановка хоста
            if (AppHost != null)
            {
                await AppHost.StopAsync();
            }
            base.OnExit(e);
        }
    }
}
