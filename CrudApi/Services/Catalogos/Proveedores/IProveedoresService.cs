using CrudApi.DTOs;
using CrudApi.DTOs.Catalogos;

namespace CrudApi.Services.Catalogos.Proveedores
{
    public interface IProveedoresService
    {
        Task<PaginacionResponse<ProveedoresResponse>> ObtenerProveedores(int numeroPagina, int registrosPorPagina);
        Task<ProveedoresResponse> ObtenerProveedorPorID(int proveedorID);
        Task<object> CrearProveedor(InsertarProveedoresRequest request, int proveedorID);
        Task<object> ActualizarProveedor(int proveedorID, ActualizarProveedoresRequest request, int usuarioModifico);
        Task<object> EliminarProveedor(int proveedorID, int usuarioElimino);
    }
}
