using CrudApi.DTOs.Usuarios;
using CrudApi.Services.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CrudApi.Controllers
{
    [Authorize]
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearUsuario([FromBody] UsuarioPerfilRequest request)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var resultado = await _usuarioService.CrearUsuarioConPerfil(request.Usuario, request.Perfil, usuarioId);
            return Ok(new { Estatus = true, Mensaje = "Usuario y perfil creados correctamente", Data = resultado });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id, [FromBody] ActualizarUsuarioRequest request)
        {
            var usuarioModifico = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var resultado = await _usuarioService.ActualizarUsuario(id, request, usuarioModifico);
            return Ok(new { Estatus = true, Mensaje = "Usuario actualizado correctamente", Data = resultado });
        }


        /// <summary>
        /// Obtiene una lista de usuarios de manera paginada.
        /// </summary>
        /// <param name="numeroPagina">Número de la página a obtener. Valor predeterminado: 1.</param>
        /// <param name="registrosPorPagina">Cantidad de registros por página. Valor predeterminado: 10.</param>        
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     GET /api/usuarios?numeroPagina=1&amp;registrosPorPagina=10
        ///
        /// La respuesta incluye la lista de usuarios, el total de registros, el número de página actual y los registros por página.
        /// </remarks>
        /// <returns>Una lista de usuarios con información de paginación.</returns>
        /// <response code="200">Si la lista de usuarios se ha obtenido correctamente.</response>
        [HttpGet]
        public async Task<IActionResult> ObtenerUsuarios([FromQuery] int numeroPagina = 1, [FromQuery] int registrosPorPagina = 10)
        {
            var usuarios = await _usuarioService.ObtenerUsuarios(numeroPagina, registrosPorPagina);

            if (usuarios.Data == null || !usuarios.Data.Any())
            {
                return Ok(new
                {
                    Estatus = true,
                    Mensaje = "No se encontraron datos",
                    Data = usuarios.Data,
                    TotalRegistros = 0,
                    NumeroPagina = numeroPagina,
                    RegistrosPorPagina = registrosPorPagina
                });
            }

            return Ok(new
            {
                Estatus = true,
                Mensaje = "Usuarios encontrados",
                Data = usuarios.Data,
                TotalRegistros = usuarios.TotalRegistros,
                NumeroPagina = usuarios.NumeroPagina,
                RegistrosPorPagina = usuarios.RegistrosPorPagina
            });
        }

        /// <summary>
        /// Obtiene los detalles de un usuario específico.
        /// </summary>
        /// <param name="id">El ID del usuario que quieres obtener.</param>
        /// <remarks>
        /// Ejemplo de solicitud:
        ///
        ///     GET /api/usuarios/1
        ///
        /// </remarks>
        /// <returns>Objeto Usuario.</returns>
        /// <response code="200">Devuelve el usuario solicitado.</response>
        /// <response code="404">Si no se encuentra el usuario.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerUsuarioPorID(int id)
        {
            var usuario = await _usuarioService.ObtenerUsuarioPorID(id);

            return Ok(new
            {
                Estatus = true,
                Mensaje = "Usuario encontrado",
                Data = usuario
            });
        }
    }
}
