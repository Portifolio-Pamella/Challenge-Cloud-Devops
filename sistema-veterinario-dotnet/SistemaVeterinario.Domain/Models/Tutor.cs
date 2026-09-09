using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace SistemaVeterinario.Domain.Models;

[Table("TB_TUTOR")]
public class Tutor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Column("ID_TUTOR")]
    [SwaggerSchema("Identificador único do tutor (gerado pelo banco)")]
    public decimal IdTutor { get; set; }

    [Required]
    [Column("NOME_TUTOR")]
    [StringLength(100)]
    [SwaggerSchema("Nome completo do tutor")]
    public string NomeTutor { get; set; } = string.Empty;

    [Required]
    [Column("CPF_TUTOR")]
    [StringLength(14)]
    [SwaggerSchema("CPF do tutor (formato: 000.000.000-00)")]
    public string CpfTutor { get; set; } = string.Empty;

    [Required]
    [Column("TELEFONE_TUTOR")] // Verifique se no seu banco a coluna não se chama diferente (ex: TEL_TUTOR)
    [StringLength(20)]
    public string TelefoneTutor { get; set; } = string.Empty;

    [Required]
    [Column("EMAIL_TUTOR")]
    [StringLength(150)]
    [SwaggerSchema("E-mail de contato")]
    public string EmailTutor { get; set; } = string.Empty;

    [Required]
    [Column("CEP_TUTOR")]
    [StringLength(10)]
    [SwaggerSchema("CEP do endereço")]
    public string CepTutor { get; set; } = string.Empty;

    [Required]
    [Column("RUA_TUTOR")]
    [StringLength(150)]
    [SwaggerSchema("Logradouro (Rua/Avenida)")]
    public string RuaTutor { get; set; } = string.Empty;

    [Required]
    [Column("NUMERO_TUTOR")]
    [StringLength(20)]
    [SwaggerSchema("Número da residência")]
    public string NumeroTutor { get; set; } = string.Empty;

    [Column("COMPLEMENTO_TUTOR")]
    [StringLength(100)]
    [SwaggerSchema("Complemento (opcional)")]
    public string ComplementoTutor { get; set; } = string.Empty;

    [Required]
    [Column("BAIRRO_TUTOR")]
    [StringLength(100)]
    [SwaggerSchema("Bairro")]
    public string BairroTutor { get; set; } = string.Empty;

    [Required]
    [Column("CIDADE_TUTOR")]
    [StringLength(100)]
    [SwaggerSchema("Cidade")]
    public string CidadeTutor { get; set; } = string.Empty;

    [Required]
    [Column("ESTADO_TUTOR")]
    [StringLength(100)]
    [SwaggerSchema("Estado (UF)")]
    public string EstadoTutor { get; set; } = string.Empty;

    [Required]
    [Column("DATA_CADASTRO_TUTOR")]
    [SwaggerSchema("Data de criação do registro")]
    public DateTime DataCadastroTutor { get; set; } = DateTime.Now;
}