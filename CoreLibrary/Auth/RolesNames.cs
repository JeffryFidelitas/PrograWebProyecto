using System.Security.Claims;

namespace CoreLibrary.Auth
{
    /// <summary>
    /// Enum que define los roles disponibles.
    /// </summary>
    public enum Roles
    {
        Administrador,
        Empleado,
        Cliente
    }

    /// <summary>
    /// Constantes con los nombres de roles, basados en el enum Roles.
    /// </summary>
    public static class RolesNames
    {
        public const string Administrador = nameof(Roles.Administrador);
        public const string Empleado = nameof(Roles.Empleado);
        public const string Cliente = nameof(Roles.Cliente);
    }

    /// <summary>
    /// Métodos auxiliares para verificar roles en ClaimsPrincipal.
    /// </summary>
    public static class RoleHelper
    {
        public static bool EsAdmin(ClaimsPrincipal user) =>
            user?.IsInRole(RolesNames.Administrador) ?? false;

        public static bool EsEmpleado(ClaimsPrincipal user) =>
            user?.IsInRole(RolesNames.Empleado) ?? false;

        public static bool EsCliente(ClaimsPrincipal user) =>
            user?.IsInRole(RolesNames.Cliente) ?? false;

        public static int? ObtenerId(ClaimsPrincipal user)
        {
            var claim = user?.FindFirst(ClaimTypes.NameIdentifier);
            return int.TryParse(claim?.Value, out var id) ? id : null;
        }

    }
}
