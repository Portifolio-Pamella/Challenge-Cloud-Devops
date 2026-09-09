using Xunit;
using Moq;
using System.Threading.Tasks;
using SistemaVeterinario.Application.Service;
using SistemaVeterinario.Domain.Models;
using SistemaVeterinario.Infrastructure.Repositories.Interfaces;

namespace SistemaVeterinario.Tests.Unit;

public class TutorServiceTests
{
    private readonly Mock<ITutorRepository> _repositoryMock;
    private readonly TutorService _service;

    public TutorServiceTests()
    {
        _repositoryMock = new Mock<ITutorRepository>();
        _service = new TutorService(_repositoryMock.Object);
    }

    [Fact]
    public async Task AddAsync_ValidTutor_CallsRepositoryAddOnce()
    {
        // Arrange
        var tutor = new Tutor
        {
            NomeTutor = "Carlos Silva",
            CpfTutor = "12345678901",
            EmailTutor = "carlos@email.com",
            CepTutor = "01001000",
            RuaTutor = "Rua A",
            NumeroTutor = "100",
            BairroTutor = "Centro",
            CidadeTutor = "São Paulo",
            EstadoTutor = "SP"
        };

        _repositoryMock.Setup(r => r.AddAsync(tutor)).Returns(Task.CompletedTask);

        // Act
        await _service.AddAsync(tutor);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(tutor), Times.Once);
    }
}