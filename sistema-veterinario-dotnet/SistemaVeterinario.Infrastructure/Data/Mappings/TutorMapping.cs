using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVeterinario.Domain.Models;

namespace SistemaVeterinario.Infrastructure.Data.Mappings;

public class TutorMapping : IEntityTypeConfiguration<Tutor>
{
    public void Configure(EntityTypeBuilder<Tutor> builder)
    {
        builder.ToTable("TB_TUTOR");

        builder.HasKey(t => t.IdTutor);

        builder.Property(t => t.IdTutor).HasColumnName("ID_TUTOR");
        builder.Property(t => t.NomeTutor).HasColumnName("NOME_TUTOR").HasMaxLength(100).IsRequired();
        builder.Property(t => t.CpfTutor).HasColumnName("CPF_TUTOR").HasMaxLength(14).IsRequired();
        builder.Property(t => t.TelefoneTutor).HasColumnName("TELEFONE_TUTOR").HasMaxLength(20).IsRequired();
        builder.Property(t => t.EmailTutor).HasColumnName("EMAIL_TUTOR").HasMaxLength(150).IsRequired();
        builder.Property(t => t.CepTutor).HasColumnName("CEP_TUTOR").HasMaxLength(10).IsRequired();
        builder.Property(t => t.RuaTutor).HasColumnName("RUA_TUTOR").HasMaxLength(150).IsRequired();
        builder.Property(t => t.NumeroTutor).HasColumnName("NUMERO_TUTOR").HasMaxLength(20).IsRequired();
        builder.Property(t => t.ComplementoTutor).HasColumnName("COMPLEMENTO_TUTOR").HasMaxLength(100);
        builder.Property(t => t.BairroTutor).HasColumnName("BAIRRO_TUTOR").HasMaxLength(100).IsRequired();
        builder.Property(t => t.CidadeTutor).HasColumnName("CIDADE_TUTOR").HasMaxLength(100).IsRequired();
        builder.Property(t => t.EstadoTutor).HasColumnName("ESTADO_TUTOR").HasMaxLength(100).IsRequired();
        builder.Property(t => t.DataCadastroTutor).HasColumnName("DATA_CADASTRO_TUTOR").IsRequired();
    }
}