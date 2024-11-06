using CrudApi.DTOs.Catalogos;
using CrudApi.Services.Catalogos.TiposDePago;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrudApi.Controllers
{
    [Authorize]
    [Route("api/tiposdepago")]
    [ApiController]
    public class TiposDePagoController: ControllerBase
    {
        private readonly ITiposDePagoService _tiposDePagoService;

        public TiposDePagoController(ITiposDePagoService tiposDePagoService)
        {
            _tiposDePagoService = tiposDePagoService;
        }

        /// <summary>
        /// Obtiene la lista de los tipos de pago paginado
        /// </summary>
        /// <param name="numeroPagina"></param>
        /// <param name="registrosPorPagina"></param>
        /// <returns>Una lista de tipos de pago con información de paginación</returns>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     GET /api/tiposdepago?numeroPagina=1&amp;registrosPorPagina=10
        ///
        /// La respuesta incluye la lista de tipos de pago, el total de registros, el número de página actual y los registros por página.
        /// </remarks>
        /// <response code="200">Si la lista de tipos de pago se ha obtenido correctamente.</response>

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int numeroPagina = 1, [FromQuery] int registrosPorPagina = 10)
        {
            var tiposDePago = await _tiposDePagoService.ObtenerTiposDePago(numeroPagina, registrosPorPagina);

            if (tiposDePago.Data == null || !tiposDePago.Data.Any())
            {
                return Ok(new
                {
                    Estatus = true,
                    Mensaje = "No se encontraron datos",
                    Data = Array.Empty<object>()
                });
            }

            return Ok(new
            {
                Estatus = true,
                Mensaje = "Tipos de Pago obtenidos correctamente",
                Data = tiposDePago.Data,
                TotalRegistros = tiposDePago.TotalRegistros,
                NumeroPagina = tiposDePago.NumeroPagina,
                RegistrosPorPagina = tiposDePago.RegistrosPorPagina
            });
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var tipoDePago = await _tiposDePagoService.ObtenerTipoDePagoPorID(id);

            if (tipoDePago == null)
            {
                return Ok(new
                {
                    Estatus = true,
                    Mensaje = "No se encontraron datos",
                    Data = Array.Empty<object>()
                });
            }

            return Ok(new
            {
                Estatus = true,
                Mensaje = "Tipo de pago encontrado",
                Data = tipoDePago
            });
        }

        [HttpPost]
        public async Task<IActionResult> Store([FromBody] InsertarTiposDePagoRequest request)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var resultado = await _tiposDePagoService.CrearTipoDePago(request, usuarioId);
            return Ok(new { Estatus = true, Mensaje = "Tipo de Pago creado correctamente", Data = "" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarTiposDePagoRequest request)
        {
            var usuarioModifico = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var resultado = await _tiposDePagoService.ActualizarTipoDePago(id, request, usuarioModifico);
            return Ok(new { Estatus = true, Mensaje = "Tipo de Pago actualizado correctamente", Data = resultado });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Destroy(int id)
        {
            var usuarioElimino = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var resultado = await _tiposDePagoService.EliminarTipoDePago(id, usuarioElimino);
            return Ok(new { Estatus = true, Mensaje = "Tipo de Pago eliminado correctamente", Data = resultado });
        }


    }
}
