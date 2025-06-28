using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClassroomApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoScheduleIdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idHorario",
                table: "Reportes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "idHorario",
                table: "Reportes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
