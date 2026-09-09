using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaVeterinario.Domain.Models;
using SistemaVeterinario.Application.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SistemaVeterinario.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetController : ControllerBase
    {
        private readonly IPetService _service;
        private readonly ILogger<PetController> _logger;

        public PetController(IPetService service, ILogger<PetController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pet>>> GetAll()
        {
            _logger.LogInformation("Iniciando listagem de todos os pets.");
            try
            {
                var pets = await _service.GetAllAsync();
                _logger.LogInformation("Listagem de pets realizada com sucesso. Total encontrados: {Count}", pets?.Count() ?? 0);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar pets.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pet>> GetById(decimal id)
        {
            _logger.LogInformation("Iniciando busca do pet por ID: {PetId}", id);
            try
            {
                var p = await _service.GetByIdAsync(id);
                if (p == null)
                {
                    _logger.LogWarning("Pet não encontrado para o ID: {PetId}", id);
                    return NotFound();
                }
                _logger.LogInformation("Pet encontrado com sucesso para o ID: {PetId}", id);
                return Ok(p);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar pet por ID: {PetId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Pet pet)
        {
            _logger.LogInformation("Iniciando cadastro de novo pet.");
            try
            {
                await _service.AddAsync(pet);
                _logger.LogInformation("Pet cadastrado com sucesso. ID: {PetId}", pet?.IdPet);
                return CreatedAtAction(nameof(GetById), new { id = pet.IdPet }, pet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar pet.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(decimal id, Pet pet)
        {
            if (id != pet.IdPet)
            {
                _logger.LogWarning("ID divergente na atualização de pet. Rota: {RouteId}, Corpo: {BodyId}", id, pet?.IdPet);
                return BadRequest("ID divergente.");
            }
            _logger.LogInformation("Iniciando atualização do pet com ID: {PetId}", id);
            try
            {
                await _service.UpdateAsync(pet);
                _logger.LogInformation("Pet atualizado com sucesso. ID: {PetId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar pet com ID: {PetId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(decimal id)
        {
            _logger.LogInformation("Iniciando exclusão do pet com ID: {PetId}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Pet excluído com sucesso. ID: {PetId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir pet com ID: {PetId}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}