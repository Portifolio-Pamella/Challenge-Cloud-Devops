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
    public class TutorController : ControllerBase
    {
        private readonly ITutorService _service;
        private readonly ILogger<TutorController> _logger;

        public TutorController(ITutorService service, ILogger<TutorController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tutor>>> GetAll()
        {
            _logger.LogInformation("Iniciando listagem de todos os tutores.");
            try
            {
                var tutores = await _service.GetAllAsync();
                _logger.LogInformation("Listagem de tutores realizada com sucesso. Total: {Count}", tutores?.Count() ?? 0);
                return Ok(tutores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar tutores.");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tutor>> GetById(decimal id)
        {
            _logger.LogInformation("Iniciando busca do tutor por ID: {TutorId}", id);
            try
            {
                var t = await _service.GetByIdAsync(id);
                if (t == null)
                {
                    _logger.LogWarning("Tutor não encontrado para o ID: {TutorId}", id);
                    return NotFound();
                }
                _logger.LogInformation("Tutor encontrado com sucesso para o ID: {TutorId}", id);
                return Ok(t);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar tutor por ID: {TutorId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Tutor tutor)
        {
            _logger.LogInformation("Iniciando cadastro de novo tutor.");
            try
            {
                await _service.AddAsync(tutor);
                _logger.LogInformation("Tutor cadastrado com sucesso. ID: {TutorId}", tutor?.IdTutor);
                return CreatedAtAction(nameof(GetById), new { id = tutor.IdTutor }, tutor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cadastrar tutor.");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(decimal id, Tutor tutor)
        {
            if (id != tutor.IdTutor)
            {
                _logger.LogWarning("IDs divergentes na atualização de tutor. Rota: {RouteId}, Corpo: {BodyId}", id, tutor?.IdTutor);
                return BadRequest("IDs divergentes.");
            }
            _logger.LogInformation("Iniciando atualização do tutor com ID: {TutorId}", id);
            try
            {
                await _service.UpdateAsync(tutor);
                _logger.LogInformation("Tutor atualizado com sucesso. ID: {TutorId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar tutor com ID: {TutorId}", id);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(decimal id)
        {
            _logger.LogInformation("Iniciando exclusão do tutor com ID: {TutorId}", id);
            try
            {
                await _service.DeleteAsync(id);
                _logger.LogInformation("Tutor excluído com sucesso. ID: {TutorId}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir tutor com ID: {TutorId}", id);
                return BadRequest(ex.Message);
            }
        }
    }
}