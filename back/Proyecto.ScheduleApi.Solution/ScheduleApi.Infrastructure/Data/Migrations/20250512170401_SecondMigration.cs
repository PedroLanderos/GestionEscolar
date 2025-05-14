using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScheduleApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dia",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "IdMateria",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "HoraInicio",
                table: "Schedules",
                newName: "Grupo");

            migrationBuilder.AddColumn<int>(
                name: "Grado",
                table: "Schedules",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubjectToSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMateria = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdHorario = table.Column<int>(type: "int", nullable: true),
                    HoraInicio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectToSchedules", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubjectToSchedules");

            migrationBuilder.DropColumn(
                name: "Grado",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "Grupo",
                table: "Schedules",
                newName: "HoraInicio");

            migrationBuilder.AddColumn<string>(
                name: "Dia",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdMateria",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdUsuario",
                table: "Schedules",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
