using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoreLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CambioDeDatosIniciales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Servicios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Servicios",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Duracion",
                table: "Servicios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Clientes",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Clientes",
                columns: new[] { "Id", "Correo", "NombreCompleto", "Telefono", "UsuarioId" },
                values: new object[] { -1, "jeffrey@gmail.com", "Jeffrey Samuel", "12345678", -2 });

            migrationBuilder.InsertData(
                table: "Servicios",
                columns: new[] { "Id", "Descripcion", "Duracion", "Nombre", "Precio" },
                values: new object[,]
                {
                    { -2, "Inspección y ajuste de frenos.", 90, "Revisión de Frenos", 49.99m },
                    { -1, "Cambio de aceite y filtro.", 60, "Cambio de Aceite", 29.99m }
                });

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: -2,
                column: "Rol",
                value: 2);

            migrationBuilder.InsertData(
                table: "Vehiculos",
                columns: new[] { "Id", "ClienteId", "Color", "Marca", "Modelo", "Placa", "Tipo" },
                values: new object[,]
                {
                    { -2, -1, "Azul", "Honda", "Civic", "XYZ789", "SUV" },
                    { -1, -1, "Rojo", "Toyota", "Corolla", "ABC123", "Sedán" }
                });

            migrationBuilder.InsertData(
                table: "Citas",
                columns: new[] { "Id", "ClienteId", "Estado", "FechaHora", "VehiculoId" },
                values: new object[,]
                {
                    { -2, -1, 1, new DateTime(2025, 4, 12, 12, 0, 0, 0, DateTimeKind.Unspecified), -2 },
                    { -1, -1, 0, new DateTime(2025, 4, 10, 12, 0, 0, 0, DateTimeKind.Unspecified), -1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Citas",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Citas",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Servicios",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Servicios",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Vehiculos",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Vehiculos",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Clientes",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DropColumn(
                name: "Duracion",
                table: "Servicios");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Servicios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Servicios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Clientes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.UpdateData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: -2,
                column: "Rol",
                value: 1);

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Apellido", "Contrasenna", "Correo", "FechaRegistro", "Nombre", "Rol" },
                values: new object[] { -1, "Montero", "12", "randall@gmail.com", new DateTime(2025, 4, 7, 12, 0, 0, 0, DateTimeKind.Unspecified), "Randall", 0 });
        }
    }
}
