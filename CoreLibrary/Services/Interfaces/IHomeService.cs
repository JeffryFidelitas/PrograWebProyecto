using CoreLibrary.Models;
using CoreLibrary.Models.ViewModels;

namespace CoreLibrary.Services.Interfaces
{
    public interface IHomeService
    {
        Task<ViewModel_Index> ObtenerIndexParaInvitado();
        Task<ViewModel_Index> ObtenerIndexParaUsuario(Usuario usuario);
    }
}
