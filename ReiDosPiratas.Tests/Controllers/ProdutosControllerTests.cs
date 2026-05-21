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
            _mockProdutoRepo = new Mock<IProdutoRepository>();
            _mockAuditRepo = new Mock<IAuditLogRepository>();

            _controller = new ProdutosController(_mockProdutoRepo.Object, _mockAuditRepo.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
            _controller.ControllerContext = new ControllerContext { HttpContext = httpContext };
        }

        [Fact]
        public async Task GetProduto_DeveRetornarNotFound_QuandoProdutoNaoExistir()
        {
            _mockProdutoRepo.Setup(repo => repo.ObterPorIdAsync(It.IsAny<int>()))
                            .ReturnsAsync((Produto)null);

            var result = await _controller.GetProduto(999);

            var actionResult = Assert.IsType<ActionResult<ProdutoResponseDTO>>(result);
            Assert.IsType<NotFoundObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetProduto_DeveRetornarOkComHateoas_QuandoProdutoExistir()
        {
            var produtoSimulado = new Produto { Produto_ID = 1, Titulo = "One Piece Vol 1", Preco = 35.90m };
            _mockProdutoRepo.Setup(repo => repo.ObterPorIdAsync(1))
                            .ReturnsAsync(produtoSimulado);

            var result = await _controller.GetProduto(1);

            var actionResult = Assert.IsType<ActionResult<ProdutoResponseDTO>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dto = Assert.IsType<ProdutoResponseDTO>(okResult.Value);

            Assert.Equal("One Piece Vol 1", dto.Titulo);
            Assert.NotEmpty(dto.Links);
            Assert.Contains(dto.Links, l => l.Rel == "self");
        }

        [Fact]
        public async Task PostProduto_DeveCriarProduto_E_RegistrarAuditoriaNoMongo()
        {
            var novoProdutoDto = new ProdutoRequestDTO
            {
                Titulo = "Naruto Vol 1",
                Preco = 29.90m,
                Estoque = 10,
                Autor = "Masashi Kishimoto"
            };

            var result = await _controller.PostProduto(novoProdutoDto);

            var actionResult = Assert.IsType<ActionResult<ProdutoResponseDTO>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var dtoRetorno = Assert.IsType<ProdutoResponseDTO>(createdResult.Value);

            Assert.Equal("Naruto Vol 1", dtoRetorno.Titulo);

            _mockProdutoRepo.Verify(repo => repo.AdicionarAsync(It.IsAny<Produto>()), Times.Once);

            _mockAuditRepo.Verify(repo => repo.RegistrarAcessoAsync(It.IsAny<AuditLog>()), Times.Once);
        }
    }
}