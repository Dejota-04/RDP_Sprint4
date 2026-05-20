using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReiDosPiratas.API.Controllers;
using ReiDosPiratas.Application.DTOs;
using ReiDosPiratas.Domain.Entities;
using ReiDosPiratas.Domain.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ReiDosPiratas.Tests.Controllers
{
    public class ProdutosControllerTests
    {
        private readonly Mock<IProdutoRepository> _mockProdutoRepo;
        private readonly Mock<IAuditLogRepository> _mockAuditRepo;
        private readonly ProdutosController _controller;

        public ProdutosControllerTests()
        {
            // Configuração dos Mocks (Simulando os bancos de dados)
            _mockProdutoRepo = new Mock<IProdutoRepository>();
            _mockAuditRepo = new Mock<IAuditLogRepository>();

            _controller = new ProdutosController(_mockProdutoRepo.Object, _mockAuditRepo.Object);

            // Simulando o HttpContext para o AuditLog (MongoDB) não quebrar ao buscar o IP
            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        }

        [Fact]
        public async Task GetProduto_DeveRetornarNotFound_QuandoProdutoNaoExistir()
        {
            // Arrange (Preparação)
            _mockProdutoRepo.Setup(repo => repo.ObterPorIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((Produto)null);

            // Act (Ação)
            var result = await _controller.GetProduto(999);

            // Assert (Verificação)
            var actionResult = Assert.IsType<ActionResult<ProdutoResponseDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetProduto_DeveRetornarOkComHateoas_QuandoProdutoExistir()
        {
            // Arrange
            var produtoSimulado = new Produto { Produto_ID = 1, Titulo = "One Piece Vol 1", Preco = 35.90m };
            _mockProdutoRepo.Setup(repo => repo.ObterPorIdAsync(1))
                            .ReturnsAsync(produtoSimulado);

            // Act
            var result = await _controller.GetProduto(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProdutoResponseDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dto = Assert.IsType<ProdutoResponseDTO>(okResult.Value);

            Assert.Equal("One Piece Vol 1", dto.Titulo);
            Assert.NotEmpty(dto.Links); // Valida se os links HATEOAS foram gerados
            Assert.Contains(dto.Links, l => l.Rel == "self");
        }

        [Fact]
        public async Task PostProduto_DeveCriarProduto_E_RegistrarAuditoriaNoMongo()
        {
            // Arrange
            var novoProdutoDto = new ProdutoRequestDTO
            {
                Titulo = "Naruto Vol 1",
                Preco = 29.90m,
                Estoque = 10,
                Autor = "Masashi Kishimoto"
            };

            // Act
            var result = await _controller.PostProduto(novoProdutoDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProdutoResponseDTO>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var dtoRetorno = Assert.IsType<ProdutoResponseDTO>(createdResult.Value);

            Assert.Equal("Naruto Vol 1", dtoRetorno.Titulo);

            // Verifica se o método de salvar no Oracle foi chamado exatamente 1 vez
            _mockProdutoRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Produto>()), Times.Once);

            // Verifica se o log do MongoDB foi disparado exatamente 1 vez
            _mockAuditRepo.Verify(repo => repo.RegistrarAcessoAsync(It.IsAny<AuditLog>()), Times.Once);
        }
    }
}