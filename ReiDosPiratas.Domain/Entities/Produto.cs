namespace ReiDosPiratas.Domain.Entities
{
    public class Produto
    {
        public long Produto_ID { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Imagem_url { get; set; } = string.Empty;

        public decimal Preco { get; set; }
        public decimal Preco_original { get; set; }

        public double? Peso { get; set; }
        public int Estoque { get; set; }
        public int Condicao_produto { get; set; }
        public float Altura { get; set; }
        public float Largura { get; set; }
        public float Profundidade { get; set; }
        public long FuncionarioId { get; set; }
        public string Autor { get; set; } = string.Empty;
        public int Categoria { get; set; }
    }
}