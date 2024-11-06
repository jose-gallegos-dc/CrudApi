using CrudApi.DTOs;
using System.Data;
using EfiWebDLL.Conexion;
using EfiWebDLL.Repositorios;
using Microsoft.Data.SqlClient;
using CrudApi.DTOs.Usuarios;
using EfiWebDLL.Excepciones;
using CrudApi.Exceptions;

namespace CrudApi.Services.Usuarios
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IRepositorioBase _repositorio;
        private readonly Conexion _conexion;

        public UsuarioService(IRepositorioBase repositorio, string connectionString)
        {
            _repositorio = repositorio;
            _conexion = new Conexion(connectionString);
        }

        public string HashearContraseña(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerificarContraseña(string passwordIngresada, string passwordHashAlmacenado)
        {
            return BCrypt.Net.BCrypt.Verify(passwordIngresada, passwordHashAlmacenado);
        }        

        public async Task<object> CrearUsuarioConPerfil(InsertarUsuarioRequest usuarioRequest, PerfilRequest perfilRequest, int usuarioID)
        {
            return await EjecutarConTryCatch(async() =>
            {
                _conexion.BeginTransaction();

                string passwordHash = HashearContraseña(usuarioRequest.Password);

                var parametrosUsuario = _conexion.CrearParametros(
                    ("@Nombre", usuarioRequest.Nombre, SqlDbType.NVarChar),
                    ("@Email", usuarioRequest.Email, SqlDbType.NVarChar),
                    ("@Password", passwordHash, SqlDbType.NVarChar),
                    ("@UsuarioCreo", usuarioID, SqlDbType.Int)
                );

                var idUsuario = await _repositorio.Crear("_Usuarios_Guardar", parametrosUsuario);

                var parametrosPerfil = _conexion.CrearParametros(
                    ("@UsuarioID", idUsuario, SqlDbType.Int),
                    ("@Rol", perfilRequest.Rol, SqlDbType.NVarChar)
                );

                await _repositorio.Crear("_Perfiles_Guardar", parametrosPerfil);

                _conexion.Commit();

                return new { UsuarioID = idUsuario };

            }, _conexion);
        }

        public async Task<object> ActualizarUsuario(int usuarioID, ActualizarUsuarioRequest usuarioRequest, int usuarioModifico)
        {
            return await EjecutarConTryCatch(async() =>
            {
                _conexion.BeginTransaction();

                string passwordHash = HashearContraseña(usuarioRequest.Password);

                var parametrosUsuario = _conexion.CrearParametros(
                    ("@UsuarioID", usuarioID, SqlDbType.Int),
                    ("@Nombre", usuarioRequest.Nombre, SqlDbType.NVarChar),
                    ("@Password", passwordHash, SqlDbType.NVarChar),
                    ("@UsuarioModifico", usuarioModifico, SqlDbType.Int)
                );

                var idUsuario = await _repositorio.Actualizar("_Usuarios_Actualizar", parametrosUsuario);

                _conexion.Commit();

                return new { UsuarioID = idUsuario };

            }, _conexion);
        }

        public async Task<object> Login(LoginRequest loginRequest)
        {

            return await EjecutarConTryCatch(async () =>
            {

                var parametros = _conexion.CrearParametros(
                    ("@Email", loginRequest.Email, SqlDbType.NVarChar)
                );

                var usuario = await _repositorio.ObtenerDatos<UsuarioResponse>("_Usuarios_Login", parametros);

                if (usuario == null || !usuario.Any())
                {
                    throw new CustomValidationException(new Dictionary<string, string[]>
                    {
                        { "Email", new[] { "El usuario no existe" } }
                    }, "Error de inicio de sesión: Usuario no encontrado");
                }

                var usuarioData = usuario.First();
                var passwordHashAlmacenado = usuarioData.Password;

                if (!VerificarContraseña(loginRequest.Password, passwordHashAlmacenado))
                {
                    throw new CustomValidationException(new Dictionary<string, string[]>
                    {
                        { "Password", new[] { "Contraseña incorrecta" } }
                    }, "Error de inicio de sesión: Contraseña incorrecta");
                }

                return usuarioData;

            }, _conexion);
        }
        
        public async Task<PaginacionResponse<UsuarioResponse>> ObtenerUsuarios(int numeroPagina, int registrosPorPagina)
        {
            return await EjecutarConTryCatch(async () =>
            {               

                var parametros = _conexion.CrearParametros(
                    ("@NumeroPagina", numeroPagina, SqlDbType.Int),
                    ("@RegistrosPorPagina", registrosPorPagina, SqlDbType.Int)
                );

                var (data, totalRegistros) = await _repositorio.ObtenerDatosPaginado<UsuarioResponse>("_Usuarios", parametros);

                return new PaginacionResponse<UsuarioResponse>(data, totalRegistros, numeroPagina, registrosPorPagina);

            }, _conexion);
        }

        public async Task<UsuarioResponse> ObtenerUsuarioPorID(int usuarioID)
        {
            return await EjecutarConTryCatch(async () =>
            {
                var parametros = _conexion.CrearParametros(("@UsuarioID", usuarioID, SqlDbType.Int));
                var usuario = await _repositorio.ObtenerDatos<UsuarioResponse>("_Usuarios_ObtenerPorID", parametros);
                return usuario.FirstOrDefault();
            }, _conexion);
        }
    }
}
