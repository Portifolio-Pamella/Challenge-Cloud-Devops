using SistemaVeterinario.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVeterinario.Infrastructure.Repositories.Interfaces;

public interface ITutorRepository
{
    Task<IEnumerable<Tutor>> GetAllAsync();
    Task<Tutor?> GetByIdAsync(decimal id);
    Task AddAsync(Tutor tutor);
    Task UpdateAsync(Tutor tutor);
    Task DeleteAsync(decimal id);
}