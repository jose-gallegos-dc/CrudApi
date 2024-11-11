using Newtonsoft.Json;

namespace CrudApi.DTOs.Catalogos
{
    public class ProveedoresRequest
    {
        public string Proveedor { get; set; }
    }

    public class ActualizarProveedoresRequest
    {
        public string Proveedor { get; set; }
    }

    public class ProveedoresResponse
    {
        public int ProveedorID { get; set; }
        public string NombreProveedor { get; set; }
        public string Razonsocial { get; set; }
        public string Estado { get; set; }
        public string Municipio { get; set; }
        public string Celular { get; set; }
        public DateTime FechaCreacion { get; set; }

        //[JsonIgnore]
        //public string nombre { get; set; }
    }
}
