using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        public static IHost AppHost { get; private set; }
        public IConfiguration Configuration { get; }

        public App()
        {
            // Настройка конфигурации
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            // Создание и настройка хоста
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Регистрация DbContext с подключением к БД
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("MySQLConnection")));

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
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            // Запуск хоста
            await AppHost.StartAsync();

            // Запуск главного окна через сервисы
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            // Остановка хоста
            await AppHost.StopAsync();
            base.OnExit(e);
        }
    }


}
