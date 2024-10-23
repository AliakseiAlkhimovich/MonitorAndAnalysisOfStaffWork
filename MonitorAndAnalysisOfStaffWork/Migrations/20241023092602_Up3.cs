using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitorAndAnalysisOfStaffWork.Migrations
{
    /// <inheritdoc />
    public partial class Up3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartName",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "PartNumber",
                table: "Details");

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "Details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Обозначение");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Наименование");

            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Номер детали - артикул");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Details");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Details");

            migrationBuilder.AddColumn<string>(
                name: "PartName",
                table: "Details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Название детали");

            migrationBuilder.AddColumn<string>(
                name: "PartNumber",
                table: "Details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Номер детали (артикул)");
        }
    }
}
