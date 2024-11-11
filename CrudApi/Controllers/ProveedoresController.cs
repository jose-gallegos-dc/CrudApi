using CrudApi.DTOs.Catalogos;
using CrudApi.Services.Catalogos.Proveedores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrudApi.Controllers
{
    [Authorize]
    [Route("api/proveedores")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly IProveedoresService _proveedoresService;

        public ProveedoresController(IProveedoresService proveedoresService)
        {
            _proveedoresService = proveedoresService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int numeroPagina = 1, [FromQuery] int registrosPorPagina = 10)
        {
            var proveedores = await _proveedoresService.ObtenerProveedores(numeroPagina, registrosPorPagina);

            if (proveedores.Data == null || !proveedores.Data.Any())
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
                Mensaje = "Proveedores obtenidos correctamente",
                Data = proveedores.Data,
                TotalRegistros = proveedores.TotalRegistros,
                NumeroPagina = proveedores.NumeroPagina,
                RegistrosPorPagina = proveedores.RegistrosPorPagina
            });
        }
    }
}
