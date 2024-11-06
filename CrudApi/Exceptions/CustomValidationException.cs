namespace CrudApi.Exceptions
{
    public class CustomValidationException : Exception
    {
        public Dictionary<string, string[]> Errores { get; }

        public CustomValidationException(Dictionary<string, string[]> errores, string mensaje = "Errores de validación")
            : base(mensaje)
        {
            Errores = errores;
        }
    }
}
