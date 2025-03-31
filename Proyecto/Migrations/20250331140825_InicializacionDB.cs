using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto.Migrations
{
    /// <inheritdoc />
    public partial class InicializacionDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LavadoId",
                table: "Citas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Lavado",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<int>(type: "int", nullable: false),
                    Duracion = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lavado", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_LavadoId",
                table: "Citas",
                column: "LavadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Citas_Lavado_LavadoId",
                table: "Citas",
                column: "LavadoId",
                principalTable: "Lavado",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Citas_Lavado_LavadoId",
                table: "Citas");

            migrationBuilder.DropTable(
                name: "Lavado");

            migrationBuilder.DropIndex(
                name: "IX_Citas_LavadoId",
                table: "Citas");

            migrationBuilder.DropColumn(
                name: "LavadoId",
                table: "Citas");
        }
    }
}
