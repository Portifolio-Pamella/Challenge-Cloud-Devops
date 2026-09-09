using SistemaVeterinario.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVeterinario.Application.Service.Interface;

public interface IPetService
{
    Task<IEnumerable<Pet>> GetAllAsync();
    Task<Pet?> GetByIdAsync(decimal id);
    Task AddAsync(Pet pet);
    Task UpdateAsync(Pet pet);
    Task DeleteAsync(decimal id);
}