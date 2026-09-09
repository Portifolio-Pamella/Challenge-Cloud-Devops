using Microsoft.EntityFrameworkCore;
using SistemaVeterinario.Domain.Models;
using SistemaVeterinario.Infrastructure.Repositories.Interfaces;
using SistemaVeterinario.Infrastructure.Data;

namespace SistemaVeterinario.Infrastructure.Repositories;

public class VeterinarioRepository : IVeterinarioRepository
{
    private readonly AppDbContext _context;

    public VeterinarioRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Veterinario>> GetAllAsync()
    {
        // Usamos AsNoTracking para evitar que o EF tente validar nulos em relacionamentos
        return await _context.Veterinarios.AsNoTracking().ToListAsync();
    }

    public async Task<Veterinario?> GetByIdAsync(decimal id)
    {
        return await _context.Veterinarios.AsNoTracking().FirstOrDefaultAsync(v => v.IdVeterinario == id);
    }

    public async Task AddAsync(Veterinario veterinario)
    {
        // Adiciona a entidade e deixa a Trigger do Oracle gerar o ID
        await _context.Veterinarios.AddAsync(veterinario);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Veterinario veterinario)
    {
        // O Update no EF Core deve ser direto. 
        // Se o ID for gerado por Trigger, não tente alterar o ID.
        _context.Veterinarios.Update(veterinario);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(decimal id)
    {
        var veterinario = await _context.Veterinarios.FindAsync(id);
        if (veterinario != null)
        {
            _context.Veterinarios.Remove(veterinario);
            await _context.SaveChangesAsync();
        }
    }
}