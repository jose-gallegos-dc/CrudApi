namespace CrudApi.DTOs
{
    public class PaginacionResponse<T>
    {
        public List<T> Data { get; set; }
        public int TotalRegistros { get; set; }
        public int NumeroPagina { get; set; }
        public int RegistrosPorPagina { get; set; }

        public PaginacionResponse(List<T> data, int totalRegistros, int numeroPagina, int registrosPorPagina)
        {
            Data = data;
            TotalRegistros = totalRegistros;
            NumeroPagina = numeroPagina;
            RegistrosPorPagina = registrosPorPagina;
        }
    }
}
