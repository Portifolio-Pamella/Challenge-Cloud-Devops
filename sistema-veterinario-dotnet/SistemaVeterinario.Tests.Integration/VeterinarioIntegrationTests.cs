using Xunit;
using System.Net;
using System.Threading.Tasks;

namespace SistemaVeterinario.Tests.Integration;

public class VeterinarioIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
	private readonly CustomWebApplicationFactory<Program> _factory;

	public VeterinarioIntegrationTests(CustomWebApplicationFactory<Program> factory)
	{
		_factory = factory;
	}

	[Fact]
	public async Task GetVeterinarios_WhenEndpointIsCalled_ReturnsSuccessOrBadRequest()
	{
		// Arrange
		var client = _factory.CreateClient();

		// Act
		var response = await client.GetAsync("/api/Veterinario");

		// Assert
		Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest);
	}
}