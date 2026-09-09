using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;

namespace SistemaVeterinario.Tests.Integration;

public class VeterinarioControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public VeterinarioControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_EndpointsReturnSuccessOrBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/Veterinario");

        // Assert
        // Valida se o endpoint responde (mesmo que retorne vazio ou erro por conta do banco, garante o fluxo HTTP)
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest);
    }
}