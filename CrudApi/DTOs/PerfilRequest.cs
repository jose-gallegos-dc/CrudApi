using System.ComponentModel.DataAnnotations;

namespace CrudApi.DTOs
{
    public class PerfilRequest
    {
        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(50, ErrorMessage = "El rol no debe superar los 50 caracteres.")]
        public string Rol { get; set; }
    }
}
