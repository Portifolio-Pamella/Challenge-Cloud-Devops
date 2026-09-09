using Xunit;
using Moq;
using System.Threading.Tasks;
using SistemaVeterinario.Application.Service;
using SistemaVeterinario.Domain.Models;
using SistemaVeterinario.Infrastructure.Repositories.Interfaces;

namespace SistemaVeterinario.Tests.Unit;

public class PetServiceTests
{
    private readonly Mock<IPetRepository> _repositoryMock;
    private readonly PetService _service;

    public PetServiceTests()
    {
        _repositoryMock = new Mock<IPetRepository>();
        _service = new PetService(_repositoryMock.Object);
    }

    [Fact]
    public async Task AddAsync_ValidPet_CallsRepositoryAddOnce()
    {
        // Arrange
        var pet = new Pet
        {
            NomePet = "Rex",
            EspeciePet = "Canino",
            RacaPet = "Labrador",
            SexoPet = "Macho",
            PesoPet = 15.5m,
            CorPet = "Preto"
        };

        _repositoryMock.Setup(r => r.AddAsync(pet)).Returns(Task.CompletedTask);

        // Act
        await _service.AddAsync(pet);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(pet), Times.Once);
    }
}