using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreLibrary.Migrations
{
    /// <inheritdoc />
    public partial class NuevasPropiedadesUsuariosYClientesYNuevaNavegacionEnContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_Placa",
                table: "Vehiculos",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Citas_FechaHora",
                table: "Citas",
                column: "FechaHora");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehiculos_Placa",
                table: "Vehiculos");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Correo",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Citas_FechaHora",
                table: "Citas");
        }
    }
}
