using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreLibrary.Migrations
{
    /// <inheritdoc />
    public partial class NuevoUsuarioAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Apellido", "Contrasenna", "Correo", "FechaRegistro", "Nombre", "Rol" },
                values: new object[] { -1, "Montero", "12", "randall@gmail.com", new DateTime(2025, 4, 7, 12, 0, 0, 0, DateTimeKind.Unspecified), "Randall", 0 });

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Correo", "NombreCompleto", "Telefono", "UsuarioId" },
                values: new object[] { -2, "randall@gmail.com", "Randall Montero", "12345678", -1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
