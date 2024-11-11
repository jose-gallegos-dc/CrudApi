using CrudApi.DTOs;
using CrudApi.DTOs.Catalogos;

namespace CrudApi.Services.Catalogos.Proveedores
{
    public interface IProveedoresService
    {
        Task<PaginacionResponse<ProveedoresResponse>> ObtenerProveedores(int numeroPagina, int registrosPorPagina);
    }
}
