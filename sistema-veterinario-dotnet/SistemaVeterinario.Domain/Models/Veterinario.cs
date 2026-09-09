using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace SistemaVeterinario.Domain.Models;

[Table("TB_VETERINARIO")]
public class Veterinario
{ // <- A chave que faltava foi adicionada aqui
    [Key]
    [Column("ID_VETERINARIO")]
    [SwaggerSchema("Identificador único")]
    public decimal IdVeterinario { get; set; }

    [Required]
    [Column("NOME_VETERINARIO")]
    [SwaggerSchema("Nome completo do profissional")]
    public string NomeVeterinario { get; set; } = string.Empty;

    [Required]
    [Column("CRM_VETERINARIO")]
    [SwaggerSchema("Registro profissional")]
    public string CrmVeterinario { get; set; } = string.Empty;

    [Required]
    [Column("ESPECIALIDADE_VETERINARIO")]
    [SwaggerSchema("Especialidade médica")]
    public string EspecialidadeVeterinario { get; set; } = string.Empty;

    [Column("TELEFONE_VETERINARIO")]
    public string? TelefoneVeterinario { get; set; }

    [Column("EMAIL_VETERINARIO")]
    public string? EmailVeterinario { get; set; }

    [Column("STATUS_VETERINARIO")]
    public string? StatusVeterinario { get; set; }

    [Required]
    [Column("DATA_CADASTRO_VETERINARIO")]
    public DateTime DataCadastroVeterinario { get; set; } = DateTime.Now;
}