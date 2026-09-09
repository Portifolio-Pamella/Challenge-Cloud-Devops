using SistemaVeterinario.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVeterinario.Application.Service.Interface;

public interface IVeterinarioService
{
    Task<IEnumerable<Veterinario>> GetAllAsync();
    Task<Veterinario?> GetByIdAsync(decimal id);
    Task AddAsync(Veterinario veterinario);
    Task UpdateAsync(Veterinario veterinario);
    Task DeleteAsync(decimal id);
}