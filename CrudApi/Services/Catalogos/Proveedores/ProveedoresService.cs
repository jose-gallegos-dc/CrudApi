using CrudApi.DTOs;
using CrudApi.DTOs.Catalogos;
using CrudApi.Exceptions;
using EfiWebDLL.Conexion;
using EfiWebDLL.Repositorios;
using System.Data;

namespace CrudApi.Services.Catalogos.Proveedores
{
    public class ProveedoresService : BaseService, IProveedoresService
    {
        private readonly IRepositorioBase _repositorio;
        private readonly Conexion _conexion;

        public ProveedoresService(IRepositorioBase repositorio, string connectionString)
        {
            _repositorio = repositorio;
            _conexion = new Conexion(connectionString);
        }

        public async Task<PaginacionResponse<ProveedoresResponse>> ObtenerProveedores(int numeroPagina, int registrosPorPagina)
        {
            return await EjecutarConTryCatch(async () =>
            {
                var parametros = _conexion.CrearParametros(
                    ("@NumeroPagina", numeroPagina, SqlDbType.Int),
                    ("@RegistrosPorPagina", registrosPorPagina, SqlDbType.Int)
                );

                var (data, totalRegistros) = await _repositorio.ObtenerDatosPaginado<ProveedoresResponse>("_Proveedores", parametros);

                return new PaginacionResponse<ProveedoresResponse>(data, totalRegistros, numeroPagina, registrosPorPagina);
            }, _conexion);
        }

        public async Task<ProveedoresResponse> ObtenerProveedorPorID(int proveedorID)
        {
            return await EjecutarConTryCatch(async () =>
            {
                var parametros = _conexion.CrearParametros(("ProveedorID", proveedorID, SqlDbType.Int));
                var resultado = await _repositorio.ObtenerDatos<ProveedoresResponse>("_Proveedores_ObtenerPorID", parametros);
                return resultado.FirstOrDefault();
            }, _conexion);
        }

        public async Task<object> CrearProveedor(InsertarProveedoresRequest request, int usuarioID)
        {
            return await EjecutarConTryCatch(async () =>
            {
                _conexion.BeginTransaction();

                var parametros = _conexion.CrearParametros(
                    ("@NombreProveedor", request.NombreProveedor, SqlDbType.NVarChar),
                    ("@RazonSocial", request.RazonSocial, SqlDbType.NVarChar),
                    ("@Estado", request.Estado, SqlDbType.NVarChar),
                    ("@Municipio", request.Municipio, SqlDbType.NVarChar),
                    ("@Celular", request.Celular, SqlDbType.NVarChar),
                    ("@UsuarioCreo", usuarioID, SqlDbType.Int)
                );
                var id = await _repositorio.Crear("_Proveedores_Guardar", parametros);

                _conexion.Commit();

                return new { ProveedorID = id };
            }, _conexion);
        }

        public async Task<object> ActualizarProveedor(int proveedorID, ActualizarProveedoresRequest request, int usuarioModifico)
        {
            return await EjecutarConTryCatch(async () =>
            {
                var parametros = _conexion.CrearParametros(
                    ("@ProveedorID", proveedorID, SqlDbType.Int),
                    ("@NombreProveedor", request.NombreProveedor, SqlDbType.NVarChar),
                    ("@RazonSocial", request.RazonSocial, SqlDbType.NVarChar),
                    ("@Estado", request.Estado, SqlDbType.NVarChar),
                    ("@Municipio", request.Municipio, SqlDbType.NVarChar),
                    ("@Celular", request.Celular, SqlDbType.NVarChar),
                    ("@UsuarioModifico", usuarioModifico, SqlDbType.Int)
                );
                await _repositorio.Actualizar("_Proveedores_Actualizar", parametros);
                return new { ProveedorID = proveedorID };
            }, _conexion);
        }

        public async Task<object> EliminarProveedor(int proveedorID, int usuarioElimino)
        {
            return await EjecutarConTryCatch(async () =>
            {
                var parametros = _conexion.CrearParametros(
                    ("@ProveedorID", proveedorID, SqlDbType.Int),
                    ("@UsuarioElimino", usuarioElimino, SqlDbType.Int)
                );
                await _repositorio.Actualizar("_proveedores_Eliminar", parametros);
                return new { ProveedorID = proveedorID };
            }, _conexion);
        }
    }
}
