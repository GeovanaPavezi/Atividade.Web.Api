using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Atividade.Web.Api;
using Atividade.Web.Api.Models;
using Xunit.Abstractions;
using Atividade.Web.Api.Controllers;

namespace Atividade.Web.Api.Tests
{
    public class CachorroControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public CachorroControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

        [Fact]
        public async Task CreateCachorro_ValidDto_ReturnsCreatedAtAction()
        {
            var client = _factory.CreateClient();
            var dto = new CachorroDto { Raca = "Labrador", Porte = "Grande" };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/cachorro", content);
            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var createdCachorro = JsonSerializer.Deserialize<CachorroDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(createdCachorro);
            Assert.Equal("Labrador", createdCachorro.Raca);
            Assert.Equal("GRANDE", createdCachorro.Porte);

            _output.WriteLine($"Resposta do Cadastro:\n{responseContent}");
        }

        [Fact]
        public async Task UpdateCachorro_ValidDto_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var dto = new CachorroDto { Raca = "Labrador", Porte = "Grande" };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var createResponse = await client.PostAsync("/api/cachorro", content);
            createResponse.EnsureSuccessStatusCode();
            var createResponseContent = await createResponse.Content.ReadAsStringAsync();
            var createdCachorro = JsonSerializer.Deserialize<CachorroDto>(createResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            _output.WriteLine($"- Cachorro criado: {createdCachorro.Raca}, {createdCachorro.Porte}");

            dto.Raca = "Labrador Atualizado";
            dto.Porte = "Médio";
            var updateContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var updateResponse = await client.PostAsync($"/api/cachorro/", updateContent);
            updateResponse.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.Created, updateResponse.StatusCode);

            var updateResponseContent = await updateResponse.Content.ReadAsStringAsync();
            var updatedCachorro = JsonSerializer.Deserialize<CachorroDto>(updateResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(updatedCachorro);
            Assert.Equal("Labrador Atualizado", updatedCachorro.Raca);
            Assert.Equal("MEDIO", updatedCachorro.Porte);

            _output.WriteLine($"Resposta da atualização:\n{updateResponseContent}");
        }

        [Fact]
        public async Task DeleteCachorro_ReturnsNoContent()
        {
            var client = _factory.CreateClient();
            var dto = new CachorroDto { Raca = "Labrador", Porte = "Grande" };
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

            var createResponse = await client.PostAsync("/api/cachorro", content);
            createResponse.EnsureSuccessStatusCode();
       


            _output.WriteLine($"- Cachorro criado:  \n{createResponse}");

            var deleteResponse = await client.DeleteAsync("/api/cachorro");
            deleteResponse.EnsureSuccessStatusCode();

            _output.WriteLine($"Cachorro deletado com sucesso: \n{deleteResponse}");
        }
    }
}
