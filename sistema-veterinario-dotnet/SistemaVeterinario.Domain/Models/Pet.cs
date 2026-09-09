using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Swashbuckle.AspNetCore.Annotations;

namespace SistemaVeterinario.Domain.Models;

[Table("TB_PET")]
public class Pet
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)] // O banco (via Trigger) define o ID
    [Column("ID_PET")]
    [SwaggerSchema("Identificador único do Pet")]
    public decimal IdPet { get; set; }

    [Required]
    [Column("ID_TUTOR")]
    [SwaggerSchema("ID do Tutor responsável pelo Pet")]
    public decimal IdTutor { get; set; }

    [ForeignKey("IdTutor")]
    [SwaggerSchema("Objeto de navegação do Tutor")]
    public Tutor? Tutor { get; set; }

    [Required]
    [Column("NOME_PET")]
    [StringLength(100)]
    [SwaggerSchema("Nome do Pet")]
    public string NomePet { get; set; } = string.Empty;

    [Required]
    [Column("ESPECIE_PET")]
    [StringLength(50)]
    [SwaggerSchema("Espécie (ex: Cachorro, Gato)")]
    public string EspeciePet { get; set; } = string.Empty;

    [Required]
    [Column("RACA_PET")]
    [StringLength(80)]
    [SwaggerSchema("Raça do Pet")]
    public string RacaPet { get; set; } = string.Empty;

    [Required]
    [Column("SEXO_PET")]
    [StringLength(10)]
    [SwaggerSchema("Sexo do Pet")]
    public string SexoPet { get; set; } = string.Empty;

    [Required]
    [Column("DATA_NASCIMENTO_PET")]
    [SwaggerSchema("Data de nascimento do Pet")]
    public DateTime DataNascimentoPet { get; set; }

    [Required]
    [Column("PESO_PET")]
    [SwaggerSchema("Peso do Pet em kg")]
    public decimal PesoPet { get; set; }

    [Required]
    [Column("COR_PET")]
    [StringLength(50)]
    [SwaggerSchema("Cor predominante")]
    public string CorPet { get; set; } = string.Empty;

    [Required]
    [Column("DATA_CADASTRO_PET")]
    [SwaggerSchema("Data de cadastro no sistema")]
    public DateTime DataCadastroPet { get; set; } = DateTime.Now;
}