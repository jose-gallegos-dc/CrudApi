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

        [HttpGet("{id}")]
        public async Task<IActionResult> Show(int id)
        {
            var proveedor = await _proveedoresService.ObtenerProveedorPorID(id);

            if (proveedor == null)
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
                Mensaje = "Proveedor encontrado",
                Data = proveedor
            });
        }

        [HttpPost]
        public async Task<IActionResult> Store([FromBody] InsertarProveedoresRequest request)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var resultado = await _proveedoresService.CrearProveedor(request, usuarioId);
            return Ok(new { Estatus = true, Mensaje = "Proveedor creado correctamente", Data = "" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ActualizarProveedoresRequest request)
        {
            var usuarioModifico = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var resultado = await _proveedoresService.ActualizarProveedor(id, request, usuarioModifico);
            return Ok(new { Estatus = true, Mensaje = "Proveedor actualizado correctamente", Data = resultado });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Destroy(int id)
        {
            var usuarioElimino = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var resultado = await _proveedoresService.EliminarProveedor(id, usuarioElimino);
            return Ok(new { Estatus = true, Mensaje = "Proveedor eliminado correctamente", Data = resultado });
        }
    }
}