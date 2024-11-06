using CrudApi.DTOs;
using CrudApi.DTOs.Catalogos;

namespace CrudApi.Services.Catalogos.TiposDePago
{
    public interface ITiposDePagoService
    {
        Task<PaginacionResponse<TiposDePagoResponse>> ObtenerTiposDePago(int numeroPagina, int registrosPorPagina);
        Task<TiposDePagoResponse> ObtenerTipoDePagoPorID(int tipoDePagoID);
        Task<object> CrearTipoDePago(InsertarTiposDePagoRequest request, int usuarioID);
        Task<object> ActualizarTipoDePago(int tipoDePagoID, ActualizarTiposDePagoRequest request, int usuarioModifico);
        Task<object> EliminarTipoDePago(int tipoDePagoID, int usuarioElimino);
    }
}
