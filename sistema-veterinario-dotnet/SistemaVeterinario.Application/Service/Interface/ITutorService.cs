using SistemaVeterinario.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemaVeterinario.Application.Service.Interface;

public interface ITutorService
{
    Task<IEnumerable<Tutor>> GetAllAsync();
    Task<Tutor?> GetByIdAsync(decimal id);
    Task AddAsync(Tutor tutor);
    Task UpdateAsync(Tutor tutor);
    Task DeleteAsync(decimal id);
}