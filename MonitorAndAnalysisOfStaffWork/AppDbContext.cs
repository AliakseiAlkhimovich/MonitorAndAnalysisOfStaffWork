using Microsoft.EntityFrameworkCore;
using MonitorAndAnalysisOfStaffWork.Entities;
using MonitorAndAnalysisOfStaffWork.Services;

namespace MonitorAndAnalysisOfStaffWork
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Автоматическое создание базы данных при необходимости
            //Database.EnsureCreated();
        }

        // Конструктор для использования EF Core при создании миграций
        public AppDbContext() : base()
        {
            // Автоматическое создание базы данных при необходимости
            //Database.EnsureCreated();
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<DetailEntity> Details { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<ManufacturedDetailEntity> ManufacturedDetails { get; set; }
        public DbSet<OperationEntity> Operations { get; set; }
        public DbSet<OperationTypeEntity> OperationTypes { get; set; }
        public DbSet<WorkTimeLogEntity> WorkTimeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity { Id = 1, Name = "admin", Title = "Администратор" },
                new RoleEntity { Id = 2, Name = "user", Title = "Пользователь" }
                );
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity { Id = 1, FullName = "Иванов Иван Иванович", PasswordHash = AuthenticationService.GetHashPasswod("admin"), RoleId = 1, Username = "admin", }
                );

            modelBuilder.Entity<RoleEntity>().Property(u => u.Id).UseIdentityColumn(3, 1);
            modelBuilder.Entity<UserEntity>().Property(u => u.Id).UseIdentityColumn(2, 1);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Конфигурация строки подключения для миграций
            if (!optionsBuilder.IsConfigured)
            {
                // Здесь указываем строку подключения вручную для миграций
                //optionsBuilder.UseSqlServer("Data Source=10.10.10.7;Initial Catalog=maaosw;Persist Security Info=True;User ID=sa;Password=666081;TrustServerCertificate=True");
                //optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=maaosw;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }
    }
}
