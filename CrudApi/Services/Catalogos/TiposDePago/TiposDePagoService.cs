using CrudApi.DTOs;
using CrudApi.DTOs.Catalogos;
using CrudApi.Exceptions;
using EfiWebDLL.Conexion;
using EfiWebDLL.Repositorios;
using System.Data;

namespace CrudApi.Services.Catalogos.TiposDePago
{
    public class TiposDePagoService: BaseService, ITiposDePagoService
    {
        private readonly IRepositorioBase _repositorio;
        private readonly Conexion _conexion;

        public TiposDePagoService(IRepositorioBase repositorio, string connectionString)
        {
            _repositorio = repositorio;
            _conexion = new Conexion(connectionString);
        }

        public async Task<PaginacionResponse<TiposDePagoResponse>> ObtenerTiposDePago(int numeroPagina, int registrosPorPagina)
        {
            return await EjecutarConTryCatch(async () =>
            {
                var parametros = _conexion.CrearParametros(
                    ("@NumeroPagina", numeroPagina, SqlDbType.Int),
                    ("@RegistrosPorPagina", registrosPorPagina, SqlDbType.Int)
                );

                var (data, totalRegistros) = await _repositorio.ObtenerDatosPaginado<TiposDePagoResponse>("_TiposDePago", parametros);

                return new PaginacionResponse<TiposDePagoResponse>(data, totalRegistros, numeroPagina, registrosPorPagina);
            }, _conexion);
        }

        public async Task<TiposDePagoResponse> ObtenerTipoDePagoPorID(int tipoDePagoID)
        {
            return await EjecutarConTryCatch(async () =>
            {
                var parametros = _conexion.CrearParametros(("TipoDePagoID", tipoDePagoID, SqlDbType.Int));
                var resultado = await _repositorio.ObtenerDatos<TiposDePagoResponse>("_TiposDePago_ObtenerPorID", parametros);
                return resultado.FirstOrDefault();
            }, _conexion);
        }

        public async Task<object> CrearTipoDePago(InsertarTiposDePagoRequest request, int usuarioID)
        {
            return await EjecutarConTryCatch(async () =>
            {
                _conexion.BeginTransaction();

                var parametros = _conexion.CrearParametros(
                    ("@TipoDePago", request.TipoDePago, SqlDbType.NVarChar),
                    ("@UsuarioCreo", usuarioID, SqlDbType.Int)
                );
                var id = await _repositorio.Crear("_TiposDePago_Guardar", parametros);

                _conexion.Commit();

                return new { TipoDePagoID = id };
            }, _conexion);
        }

        public async Task<object> ActualizarTipoDePago(int tipoDePagoID, ActualizarTiposDePagoRequest request, int usuarioModifico)
        {
            return await EjecutarConTryCatch(async () =>
            {
                var parametros = _conexion.CrearParametros(
                    ("@TipoDePagoID", tipoDePagoID, SqlDbType.Int),
                    ("@TipoDePago", request.TipoDePago, SqlDbType.NVarChar),
                    ("@Estatus", request.Estatus, SqlDbType.Bit),
                    ("@UsuarioModifico", usuarioModifico, SqlDbType.Int)
                );
                await _repositorio.Actualizar("_TiposDePago_Actualizar", parametros);
                return new { TipoDePagoID = tipoDePagoID };
            }, _conexion);
        }

        public async Task<object> EliminarTipoDePago(int tipoDePagoID, int usuarioElimino)
        {
            return await EjecutarConTryCatch(async () =>
            {
                var parametros = _conexion.CrearParametros(
                    ("@TipoDePagoID", tipoDePagoID, SqlDbType.Int),
                    ("@UsuarioElimino", usuarioElimino, SqlDbType.Int)
                );
                await _repositorio.Actualizar("_TiposDePago_Eliminar", parametros);
                return new { TipoDePagoID = tipoDePagoID };
            }, _conexion);
        }
    }
}
