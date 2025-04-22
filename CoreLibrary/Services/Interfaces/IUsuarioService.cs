using CoreLibrary.Models;
using CoreLibrary.Models.ViewModels;

namespace CoreLibrary.Services.Interfaces
{
    public interface IUsuarioService
    {
        #region Métodos de Búsqueda
        Task<Usuario?> BuscarPorCorreo(string correo);
        Task<Usuario?> ObtenerPorIdAsync(int? id);
        Task<Cliente?> ObtenerClientePorIdAsync(int usuarioId);
        Task<ViewModel_UsuarioPerfil?> ObtenerPerfilPorIdAsync(int usuarioId);
        Task<Usuario?> ObtenerPorCredencialesAsync(string correo, string contrasenna);
        #endregion

        #region Métodos de Validación
        Task<bool> ValidarCredencialesAsync(string correo, string contrasenna);
        #endregion

        #region Métodos de Gestión
        Task<IEnumerable<Usuario>> ObtenerTodosAsync(Usuario usuario);
        Task<bool> CrearAsync(ViewModel_UsuarioRegister usuario);
        Task<bool> CambiarContrasennaAsync(Usuario usuario, string nuevaContrasenna);
        Task<bool> VerificarContrasennaAsync(Usuario usuario, string contrasenna);
        Task<bool> ActualizarAsync(Usuario usuario);
        Task<bool> EliminarAsync(int id);
        #endregion
    }

}
