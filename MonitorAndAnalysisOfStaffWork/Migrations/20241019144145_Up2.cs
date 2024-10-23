using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitorAndAnalysisOfStaffWork.Migrations
{
    /// <inheritdoc />
    public partial class Up2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkTimeLogId",
                table: "WorkTimeLogs",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WorkTimeLogs",
                newName: "WorkTimeLogId");
        }
    }
}
