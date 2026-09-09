using SistemaVeterinario.Domain.Models;
using SistemaVeterinario.Infrastructure.Repositories.Interfaces;
using SistemaVeterinario.Application.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace SistemaVeterinario.Application.Service;

public class VeterinarioService : IVeterinarioService
{
    private readonly IVeterinarioRepository _repository;

    public VeterinarioService(IVeterinarioRepository repository) => _repository = repository;

    public async Task<IEnumerable<Veterinario>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Veterinario?> GetByIdAsync(decimal id) => await _repository.GetByIdAsync(id);

    public async Task AddAsync(Veterinario veterinario)
    {
        try
        {
            await _repository.AddAsync(veterinario);
        }
        catch (DbUpdateException ex)
        {
            var innerError = ex.InnerException?.Message ?? ex.Message;
            throw new System.Exception($"Erro detalhado do Oracle: {innerError}");
        }
    }

    public async Task UpdateAsync(Veterinario veterinario) => await _repository.UpdateAsync(veterinario);

    public async Task DeleteAsync(decimal id) => await _repository.DeleteAsync(id);
}