using Microsoft.EntityFrameworkCore;
using SistemaVeterinario.Domain.Models;
using SistemaVeterinario.Infrastructure.Repositories.Interfaces;
using SistemaVeterinario.Infrastructure.Data;

namespace SistemaVeterinario.Infrastructure.Repositories;

public class PetRepository : IPetRepository
{
    private readonly AppDbContext _context;

    public PetRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Pet>> GetAllAsync()
    {
        return await _context.Pets.AsNoTracking().Include(p => p.Tutor).ToListAsync();
    }

    public async Task<Pet?> GetByIdAsync(decimal id)
    {
        return await _context.Pets.AsNoTracking().Include(p => p.Tutor).FirstOrDefaultAsync(p => p.IdPet == id);
    }

    public async Task AddAsync(Pet pet)
    {
        // Isso evita que o EF tente inserir um Tutor que já existe no banco
        pet.Tutor = null;
        await _context.Pets.AddAsync(pet);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pet pet)
    {
        _context.Pets.Update(pet);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(decimal id)
    {
        var pet = await _context.Pets.FindAsync(id);
        if (pet != null)
        {
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();
        }
    }
}