using Newtonsoft.Json;

namespace CrudApi.DTOs.Usuarios
{
    public class UsuarioResponse
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
    }
}
