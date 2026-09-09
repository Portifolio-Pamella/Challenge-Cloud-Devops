using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaVeterinario.Domain.Models;

namespace SistemaVeterinario.Infrastructure.Data.Mappings
{
    public class PetMapping : IEntityTypeConfiguration<Pet>
    {
        public void Configure(EntityTypeBuilder<Pet> builder)
        {
            builder.ToTable("TB_PET");

            builder.HasKey(p => p.IdPet);

            builder.Property(p => p.IdPet).HasColumnName("ID_PET");

            builder.Property(p => p.IdTutor).HasColumnName("ID_TUTOR").IsRequired();

            builder.Property(p => p.NomePet)
                .HasColumnName("NOME_PET")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.EspeciePet)
                .HasColumnName("ESPECIE_PET")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.RacaPet)
                .HasColumnName("RACA_PET")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(p => p.SexoPet)
                .HasColumnName("SEXO_PET")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(p => p.DataNascimentoPet)
                .HasColumnName("DATA_NASCIMENTO_PET")
                .IsRequired();

            builder.Property(p => p.PesoPet)
                .HasColumnName("PESO_PET")
                .HasColumnType("decimal(18,2)") // Recomendado para evitar precisão infinita
                .IsRequired();

            builder.Property(p => p.CorPet)
                .HasColumnName("COR_PET")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.DataCadastroPet)
                .HasColumnName("DATA_CADASTRO_PET")
                .IsRequired();

            // Relacionamento
            builder.HasOne(p => p.Tutor)
                .WithMany()
                .HasForeignKey(p => p.IdTutor)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}