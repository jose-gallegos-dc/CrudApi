using CrudApi.DTOs;
using CrudApi.DTOs.Usuarios;
using CrudApi.Services;
using CrudApi.Services.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudApi.Controllers
{
    #pragma warning disable CS1591
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IJwtService _jwtService;

        public AuthController(IUsuarioService usuarioService, IJwtService jwtService)
        {
            _usuarioService = usuarioService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Inicia sesión y genera un token JWT.
        /// </summary>
        /// <param name="loginRequest">Datos del usuario para iniciar sesión.</param>
        /// <returns>Token JWT si las credenciales son válidas.</returns>

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var resultadoLogin = await _usuarioService.Login(loginRequest);

            if (resultadoLogin != null)
            {
                var token = _jwtService.GenerarToken(((UsuarioResponse)resultadoLogin).UsuarioID);

                return Ok(new
                {
                    Estatus = true,
                    Mensaje = "Login exitoso",
                    Token = token
                });
            }

            return Unauthorized(new { Estatus = "Error", Mensaje = "Credenciales incorrectas" });
        }
    }
}
