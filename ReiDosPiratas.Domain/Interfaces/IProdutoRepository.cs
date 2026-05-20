using ReiDosPiratas.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReiDosPiratas.Domain.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto> ObterPorIdAsync(int id);
        Task<IEnumerable<Produto>> ObterTodosAsync();
        Task AdicionarAsync(Produto produto);
        Task AtualizarAsync(Produto produto);
        Task RemoverAsync(int id);
    }
}