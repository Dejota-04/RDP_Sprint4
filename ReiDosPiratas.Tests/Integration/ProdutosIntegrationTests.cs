using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ReiDosPiratas.Tests.Integration
{
    // WebApplicationFactory sobe a API inteira em memória para simular requisições HTTP reais
    public class ProdutosIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProdutosIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetProdutos_DeveRetornarSucesso_QuandoRotaChamada()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/produtos");

            // Assert
            response.EnsureSuccessStatusCode(); // Valida se o status é 200-299
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}