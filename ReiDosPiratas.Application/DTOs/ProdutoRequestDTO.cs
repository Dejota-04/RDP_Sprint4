using System.ComponentModel.DataAnnotations;

namespace ReiDosPiratas.Application.DTOs
{
    public class ProdutoRequestDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(255)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "A URL da imagem é obrigatória.")]
        public string Imagem_url { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Required]
        public decimal Preco_original { get; set; }

        public double? Peso { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
        public int Estoque { get; set; }

        [Required]
        public int Condicao_produto { get; set; }

        [Required]
        public float Altura { get; set; }

        [Required]
        public float Largura { get; set; }

        [Required]
        public float Profundidade { get; set; }

        [Required]
        public long FuncionarioId { get; set; }

        [Required(ErrorMessage = "O Autor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome do autor não pode exceder 100 caracteres.")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public int Categoria { get; set; }
    }
}