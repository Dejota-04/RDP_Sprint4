using System.Collections.Generic;

namespace ReiDosPiratas.Application.DTOs
{
    public class PagedResponseDTO<T>
    {
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
        public int TamanhoPagina { get; set; }
        public int TotalItens { get; set; }
        public IEnumerable<T> Dados { get; set; } = new List<T>();
    }
}