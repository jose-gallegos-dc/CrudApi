using CrudApi.DTOs;
using CrudApi.DTOs.Usuarios;

namespace CrudApi.Services.Usuarios
{
    public interface IUsuarioService
    {
        Task<object> CrearUsuarioConPerfil(InsertarUsuarioRequest usuarioRequest, PerfilRequest perfilRequest, int usuarioID);
        Task<object> ActualizarUsuario(int usuarioID, ActualizarUsuarioRequest usuarioRequest, int usuarioModifico);
        Task<object> Login(LoginRequest loginRequest);
        Task<PaginacionResponse<UsuarioResponse>> ObtenerUsuarios(int numeroPagina, int registrosPorPagina);
        Task<UsuarioResponse> ObtenerUsuarioPorID(int usuarioID);
    }
}
