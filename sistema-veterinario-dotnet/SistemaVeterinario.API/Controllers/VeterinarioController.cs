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
    public class VeterinarioController : ControllerBase
    {
        private readonly IVeterinarioService _service;
        private readonly ILogger<VeterinarioController> _logger;

        public VeterinarioController(IVeterinarioService service, ILogger<VeterinarioController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Veterinario>>> GetAll()
        {
            _logger.LogInformation("Iniciando listagem de todos os veterinários.");
            try
            {
                var veterinarios = await _service.GetAllAsync();
                _logger.LogInformation("Listagem de veterinários realizada com sucesso. Total: {Count}", veterinarios?.Count() ?? 0);
                return Ok(veterinarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar veterinários.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Veterinario>> GetById(decimal id)
        {
            _logger.LogInformation("Iniciando busca do veterinário por ID: {VetId}", id);
            try
            {
                var v = await _service.GetByIdAsync(id);
                if (v == null)
                {
                    _logger.LogWarning("Veterinário não encontrado para o ID: {VetId}", id);
                    return NotFound("Veterinário não encontrado.");
                }
                _logger.LogInformation("Veterinário encontrado com sucesso para o ID: {VetId}", id);
                return Ok(v);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar veterinário por ID: {VetId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Veterinario v)
        {
            _logger.LogInformation("Iniciando cadastro de novo veterinário.");
            try
            {
                await _service.AddAsync(v);
                _logger.LogInformation("Veterinário cadastrado com sucesso. ID: {VetId}", v?.IdVeterinario);
                return CreatedAtAction(nameof(GetById), new { id = v.IdVeterinario }, v);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar veterinário.");
                return BadRequest($"Erro ao cadastrar: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(decimal id, Veterinario v)
        {
            if (id != v.IdVeterinario)
            {
                _logger.LogWarning("IDs divergentes na atualização de veterinário. Rota: {RouteId}, Corpo: {BodyId}", id, v?.IdVeterinario);
                return BadRequest("IDs divergentes.");
            }
            _logger.LogInformation("Iniciando atualização do veterinário com ID: {VetId}", id);
            try
            {
                await _service.UpdateAsync(v);
                _logger.LogInformation("Veterinário atualizado com sucesso. ID: {VetId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar veterinário com ID: {VetId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(decimal id)
        {
            _logger.LogInformation("Iniciando exclusão do veterinário com ID: {VetId}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Veterinário excluído com sucesso. ID: {VetId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir veterinário com ID: {VetId}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}