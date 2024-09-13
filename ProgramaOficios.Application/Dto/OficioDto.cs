namespace ProgramaOficios.Application.Dto
{
    public class OficioDto
    {
        public int Id { get; set; }
        public string? Numero { get; set; }
        public int Ano { get; set; }
        public string? Unidade { get; set; }
        public DateTime Data { get; set; }
        public string? ArquivoUrl { get; set; } 
    }
}
