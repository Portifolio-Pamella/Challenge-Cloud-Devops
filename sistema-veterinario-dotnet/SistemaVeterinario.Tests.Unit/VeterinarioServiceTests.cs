using Xunit;
using Moq;
using System.Threading.Tasks;
using SistemaVeterinario.Application.Service;
using SistemaVeterinario.Domain.Models;
using SistemaVeterinario.Infrastructure.Repositories.Interfaces;

namespace SistemaVeterinario.Tests.Unit;

public class VeterinarioServiceTests
{
    private readonly Mock<IVeterinarioRepository> _repositoryMock;
    private readonly VeterinarioService _service;

    public VeterinarioServiceTests()
    {
        _repositoryMock = new Mock<IVeterinarioRepository>();
        _service = new VeterinarioService(_repositoryMock.Object);
    }

    [Fact]
    public async Task AddAsync_ValidVeterinario_CallsRepositoryAddOnce()
    {
        // Arrange
        var veterinario = new Veterinario
        {
            NomeVeterinario = "Dra. Ana",
            CrmVeterinario = "CRM-9999",
            EmailVeterinario = "ana@test.com"
        };

        _repositoryMock.Setup(r => r.AddAsync(veterinario)).Returns(Task.CompletedTask);

        // Act
        await _service.AddAsync(veterinario);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(veterinario), Times.Once);
    }
}