using CrudApi.Exceptions;
using EfiWebDLL.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CrudApi.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is CustomValidationException ex)
            {
                var responseObj = new
                {
                    Estatus = false,
                    Mensaje = context.Exception.Message,
                    Errores = ex.Errores
                };

                context.Result = new BadRequestObjectResult(responseObj);
                context.ExceptionHandled = true;
            }
            else if (context.Exception is CustomSqlException sqlEx && sqlEx.EsAdvertencia)
            {
                var mensaje = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sqlEx.SqlException.Message)["Mensaje"];
                var responseObj = new
                {
                    Estatus = false,
                    Mensaje = mensaje
                };

                context.Result = new ObjectResult(responseObj)
                {
                    StatusCode = 403
                };
                context.ExceptionHandled = true;
            }
            else
            {
                context.Result = new ObjectResult(new { Estatus = "Error", Mensaje = context.Exception.Message })
                {
                    StatusCode = 500
                };
                context.ExceptionHandled = true;
            }
        }
    }
}
