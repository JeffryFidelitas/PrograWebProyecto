using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Models;
using CoreLibrary.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CoreLibrary.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario?> BuscarPorCorreo(string correo);

        Task<Usuario?> BuscarPorCredenciales(string correo, string contrasenna);

        Task<IEnumerable<Usuario>> ObtenerTodosAsync(Usuario usuario);

        Task<Usuario?> ObtenerPorIdAsync(int id);

        Task<ViewModel_UsuarioPerfilCompleto> ObtenerPerfilPorIdAsync(int id);

        Task CrearAsync(ViewModel_UsuarioRegister usuario);

        Task ActualizarAsync(Usuario usuario);

        Task EliminarAsync(int id);
    }
}
