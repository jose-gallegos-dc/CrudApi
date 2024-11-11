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
    }
}
