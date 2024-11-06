using Newtonsoft.Json;

namespace CrudApi.DTOs.Catalogos
{
    public class InsertarTiposDePagoRequest
    {
        public string TipoDePago { get; set; }
    }

    public class ActualizarTiposDePagoRequest
    {
        public string TipoDePago { get; set; }
        public bool Estatus { get; set; }
    }

    public class TiposDePagoResponse
    {
        public int TipoDePagoID { get; set; }
        public string TipoDePago { get; set; }
        public bool Estatus { get; set; }
        public DateTime FechaCreacion { get; set; }

        [JsonIgnore]
        public string nombre { get; set; }
    }
}
