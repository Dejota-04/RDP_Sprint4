using Microsoft.EntityFrameworkCore;
using ReiDosPiratas.Domain.Entities;
using ReiDosPiratas.Domain.Interfaces;
using ReiDosPiratas.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReiDosPiratas.Infrastructure.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProdutoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Produto> ObterPorIdAsync(int id)
        {
            return await _context.Produtos.FindAsync((long)id);
        }

        public async Task<IEnumerable<Produto>> ObterTodosAsync()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task AdicionarAsync(Produto produto)
        {
            await _context.Produtos.AddAsync(produto);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Produto produto)
        {
            _context.Produtos.Update(produto);
            await _context.SaveChangesAsync();
        }

        public async Task RemoverAsync(int id)
        {
            var produto = await ObterPorIdAsync(id);
            if (produto != null)
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
            }
        }
    }
}