using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVeterinario.Domain.Models;

namespace SistemaVeterinario.Infrastructure.Data.Mappings
{
    public class VeterinarioMapping : IEntityTypeConfiguration<Veterinario>
    {
        public void Configure(EntityTypeBuilder<Veterinario> builder)
        {
            builder.ToTable("TB_VETERINARIO");

            builder.HasKey(v => v.IdVeterinario);

            builder.Property(v => v.IdVeterinario).HasColumnName("ID_VETERINARIO");

            builder.Property(v => v.NomeVeterinario)
                .HasColumnName("NOME_VETERINARIO")
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(v => v.CrmVeterinario)
                .HasColumnName("CRM_VETERINARIO")
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(v => v.EspecialidadeVeterinario)
                .HasColumnName("ESPECIALIDADE_VETERINARIO")
                .HasMaxLength(100);

            builder.Property(v => v.TelefoneVeterinario)
                .HasColumnName("TELEFONE_VETERINARIO")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(v => v.EmailVeterinario)
                .HasColumnName("EMAIL_VETERINARIO")
                .HasMaxLength(150);

            builder.Property(v => v.StatusVeterinario)
                .HasColumnName("STATUS_VETERINARIO")
                .HasMaxLength(20)
                .HasDefaultValue("ATIVO")
                .IsRequired();

            builder.Property(v => v.DataCadastroVeterinario)
                .HasColumnName("DATA_CADASTRO_VETERINARIO")
                .IsRequired();
        }
    }
}