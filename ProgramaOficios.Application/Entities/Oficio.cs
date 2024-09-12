namespace ProgramaOficios.Application.Entities
{
    public class Oficio
    {
        public int Id { get; set; }

        // Definir como anuláveis ou inicializar corretamente
        public string Numero { get; set; } = string.Empty; // Inicializado com string vazia
        public int Ano { get; set; }
        public string Unidade { get; set; } = string.Empty; // Inicializado com string vazia
        public DateTime Data { get; set; }
        public string? ArquivoUrl { get; set; } // Permitir nulo se não for obrigatório

        // Construtor e métodos de negócio
        public Oficio(string numero, int ano, string unidade, DateTime data)
        {
            // Validações e regras de negócio
            Numero = numero;
            Ano = ano;
            Unidade = unidade;
            Data = data;

        }
    }
}
