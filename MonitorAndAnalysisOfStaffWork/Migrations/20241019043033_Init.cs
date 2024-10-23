using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MonitorAndAnalysisOfStaffWork.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор детали")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Номер детали (артикул)"),
                    PartName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Название детали")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Id);
                },
                comment: "Деталь");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор сотрудника")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Полное имя сотрудника"),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Должность сотрудника"),
                    PersonnelNumber = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Табельный номер сотрудника")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                },
                comment: "Сотрудник");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор роли")
                        .Annotation("SqlServer:Identity", "3, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Название роли (например, Администратор, Пользователь)"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "Описание роли")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                },
                comment: "Роль пользователя в системе");

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор операции")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Название операции (например, токарная, фрезерная и т.д.)"),
                    StandardTime = table.Column<TimeSpan>(type: "time", nullable: false, comment: "Стандартное время выполнения операции"),
                    DetailId = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор детали, к которой относится операция")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Details_DetailId",
                        column: x => x.DetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Операция");

            migrationBuilder.CreateTable(
                name: "ManufacturedDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор произведенной детали")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Количество изготовленных деталей"),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата изготовления деталей"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор сотрудника, который произвел детали"),
                    DetailId = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор детали, которая была произведена")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturedDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManufacturedDetails_Details_DetailId",
                        column: x => x.DetailId,
                        principalTable: "Details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManufacturedDetails_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Изготовленная деталь");

            migrationBuilder.CreateTable(
                name: "WorkTimeLogs",
                columns: table => new
                {
                    WorkTimeLogId = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор записи рабочего времени")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата записи рабочего времени"),
                    HoursWorked = table.Column<TimeSpan>(type: "time", nullable: false, comment: "Количество отработанных часов"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор сотрудника, к которому относится запись")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTimeLogs", x => x.WorkTimeLogId);
                    table.ForeignKey(
                        name: "FK_WorkTimeLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Рабочее время сотрудника");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор пользователя")
                        .Annotation("SqlServer:Identity", "2, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Имя пользователя"),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Хэш пароля пользователя"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Полное имя пользователя"),
                    RoleId = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор роли пользователя")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Учетную запись пользователя");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "Title" },
                values: new object[,]
                {
                    { 1, "admin", "Администратор" },
                    { 2, "user", "Пользователь" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "PasswordHash", "RoleId", "Username" },
                values: new object[] { 1, "Иванов Иван Иванович", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", 1, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturedDetails_DetailId",
                table: "ManufacturedDetails",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturedDetails_EmployeeId",
                table: "ManufacturedDetails",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_DetailId",
                table: "Operations",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTimeLogs_EmployeeId",
                table: "WorkTimeLogs",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManufacturedDetails");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkTimeLogs");

            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
