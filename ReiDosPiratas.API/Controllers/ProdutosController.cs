using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReiDosPiratas.Application.DTOs;
using ReiDosPiratas.Domain.Entities;
using ReiDosPiratas.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReiDosPiratas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public ProdutosController(IProdutoRepository produtoRepository, IAuditLogRepository auditLogRepository)
        {
            _produtoRepository = produtoRepository;
            _auditLogRepository = auditLogRepository;
        }

[HttpGet]
  public async Task<ActionResult<PagedResponseDTO<ProdutoResponseDTO>>> GetProdutos(
      [FromQuery] int pagina = 1,
      [FromQuery] int tamanhoPagina = 10,
      [FromQuery] int? categoria = null,
      [FromQuery] string? ordenarPor = null)
  {
      var produtos = await _produtoRepository.ObterTodosAsync();

      if (categoria.HasValue)
          produtos = produtos.Where(p => p.Categoria == categoria.Value);

      produtos = ordenarPor?.ToLower() switch
      {
          "preco_asc" => produtos.OrderBy(p => p.Preco),
          "preco_desc" => produtos.OrderByDescending(p => p.Preco),
          "titulo" => produtos.OrderBy(p => p.Titulo),
          _ => produtos.OrderBy(p => p.Produto_ID)
      };

      var totalItens = produtos.Count();
      var totalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoPagina);
      var itensPaginados = produtos.Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina);

      var dados = itensPaginados.Select(MapearParaDTO).ToList();

      return Ok(new PagedResponseDTO<ProdutoResponseDTO>
      {
          PaginaAtual = pagina,
          TotalPaginas = totalPaginas,
          TamanhoPagina = tamanhoPagina,
          TotalItens = totalItens,
          Dados = dados
      });
  }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoResponseDTO>> GetProduto(long id)
        {
            var produto = await _produtoRepository.ObterPorIdAsync((int)id);

            if (produto == null) return NotFound(new { mensagem = "Mangá não encontrado." });

            return Ok(MapearParaDTO(produto));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProdutoResponseDTO>> PostProduto(ProdutoRequestDTO dto)
        {
            var produto = new Produto
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Imagem_url = dto.Imagem_url,
                Preco = dto.Preco,
                Preco_original = dto.Preco_original,
                Peso = dto.Peso,
                Estoque = dto.Estoque,
                Condicao_produto = dto.Condicao_produto,
                Altura = dto.Altura,
                Largura = dto.Largura,
                Profundidade = dto.Profundidade,
                FuncionarioId = dto.FuncionarioId,
                Autor = dto.Autor,
                Categoria = dto.Categoria
            };

            await _produtoRepository.AdicionarAsync(produto);

            await RegistrarAuditoriaMongo("CREATE", $"Produto '{produto.Titulo}' cadastrado.");

            var responseDto = MapearParaDTO(produto);

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Produto_ID }, responseDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProduto(long id, ProdutoRequestDTO dto)
        {
            var produtoExistente = await _produtoRepository.ObterPorIdAsync((int)id);
            if (produtoExistente == null) return NotFound();

            produtoExistente.Titulo = dto.Titulo;
            produtoExistente.Preco = dto.Preco;
            produtoExistente.Estoque = dto.Estoque;
            produtoExistente.Autor = dto.Autor;

            await _produtoRepository.AtualizarAsync(produtoExistente);

            await RegistrarAuditoriaMongo("UPDATE", $"Produto ID {id} alterado.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduto(long id)
        {
            var produtoExistente = await _produtoRepository.ObterPorIdAsync((int)id);
            if (produtoExistente == null) return NotFound();

            await _produtoRepository.RemoverAsync((int)id);

            await RegistrarAuditoriaMongo("DELETE", $"Produto ID {id} removido.");

            return NoContent();
        }

        private ProdutoResponseDTO MapearParaDTO(Produto produto)
        {
            var dto = new ProdutoResponseDTO
            {
                Produto_ID = produto.Produto_ID,
                Titulo = produto.Titulo,
                Descricao = produto.Descricao,
                Imagem_url = produto.Imagem_url,
                Preco = produto.Preco,
                Preco_original = produto.Preco_original,
                Peso = produto.Peso,
                Estoque = produto.Estoque,
                Condicao_produto = produto.Condicao_produto,
                Altura = produto.Altura,
                Largura = produto.Largura,
                Profundidade = produto.Profundidade,
                FuncionarioId = produto.FuncionarioId,
                Autor = produto.Autor,
                Categoria = produto.Categoria
            };

            dto.Links.Add(new HateoasLink($"/api/produtos/{produto.Produto_ID}", "self", "GET"));
            dto.Links.Add(new HateoasLink($"/api/produtos/{produto.Produto_ID}", "update_produto", "PUT"));
            dto.Links.Add(new HateoasLink($"/api/produtos/{produto.Produto_ID}", "delete_produto", "DELETE"));

            return dto;
        }

        private async Task RegistrarAuditoriaMongo(string acao, string detalhe)
        {
            var log = new AuditLog
            {
                Acao = acao,
                Usuario = "Admin_Painel",
                DataAcesso = DateTime.UtcNow,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Desconhecido"
            };

            await _auditLogRepository.RegistrarAcessoAsync(log);
        }
    }
}