using EfiWebDLL.Conexion;
using EfiWebDLL.Excepciones;
using Microsoft.Data.SqlClient;

namespace CrudApi.Exceptions
{
    public abstract class BaseService
    {
        protected async Task<T> EjecutarConTryCatch<T>(Func<Task<T>> funcion, Conexion conexion)
        {
            try
            {
                return await funcion();
            }
            catch (CustomSqlException ex)
            {
                conexion.Rollback();

                if (ex.EsAdvertencia)
                {
                    throw;
                }

                var validationError = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(ex.SqlException.Message);

                var errores = new Dictionary<string, string[]>
                {
                    { validationError["Campo"], new[] { validationError["Mensaje"] } }
                };

                throw new CustomValidationException(errores);
            }
            catch (SqlException ex)
            {
                conexion.Rollback();
                throw new Exception("Error de SQL Server: " + ex.Message);
            }
            catch (Exception ex)
            {
                conexion.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}
