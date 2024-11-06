using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace CrudApi.Filters
{
    public class ValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        //kvp => kvp.Key,//Si se requiere incluir el nombre del Modelo + El campo habilitar esta línea y comentar la línea siguiente
                        kvp => kvp.Key.Split('.').Last(),
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                var responseObj = new
                {
                    Estatus = false,
                    Mensaje = "Errores de validación",
                    Errores = errors
                };

                context.Result = new BadRequestObjectResult(responseObj);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
