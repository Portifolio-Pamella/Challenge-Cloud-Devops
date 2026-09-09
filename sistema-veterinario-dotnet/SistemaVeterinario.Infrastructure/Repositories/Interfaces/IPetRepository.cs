using SistemaVeterinario.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVeterinario.Infrastructure.Repositories.Interfaces;

public interface IPetRepository
{
    Task<IEnumerable<Pet>> GetAllAsync();
    Task<Pet?> GetByIdAsync(decimal id);
    Task AddAsync(Pet pet);
    Task UpdateAsync(Pet pet);
    Task DeleteAsync(decimal id);
}