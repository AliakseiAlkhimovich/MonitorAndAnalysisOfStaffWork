using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitorAndAnalysisOfStaffWork.Migrations
{
    /// <inheritdoc />
    public partial class Up1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationName",
                table: "Operations");

            migrationBuilder.RenameColumn(
                name: "PersonnelNumber",
                table: "Employees",
                newName: "Number");

            migrationBuilder.AddColumn<int>(
                name: "OperationTypeId",
                table: "Operations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Идентификатор типа операции");

            migrationBuilder.CreateTable(
                name: "OperationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Идентификатор типа операции")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Название операции")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_OperationTypeId",
                table: "Operations",
                column: "OperationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_OperationTypes_OperationTypeId",
                table: "Operations",
                column: "OperationTypeId",
                principalTable: "OperationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_OperationTypes_OperationTypeId",
                table: "Operations");

            migrationBuilder.DropTable(
                name: "OperationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Operations_OperationTypeId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "OperationTypeId",
                table: "Operations");

            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Employees",
                newName: "PersonnelNumber");

            migrationBuilder.AddColumn<string>(
                name: "OperationName",
                table: "Operations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                comment: "Название операции (например, токарная, фрезерная и т.д.)");
        }
    }
}
