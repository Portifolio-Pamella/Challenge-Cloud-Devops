using Microsoft.EntityFrameworkCore;
using SistemaVeterinario.Domain.Models;

namespace SistemaVeterinario.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tutor> Tutores { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<Veterinario> Veterinarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Mapeamento Veterinário
        modelBuilder.Entity<Veterinario>(entity =>
        {
            entity.ToTable("TB_VETERINARIO");
            entity.HasKey(v => v.IdVeterinario);

            // Alterado para ValueGeneratedNever para evitar envio de nulo, 
            // permitindo que a sequence do banco atue ou o insert ocorra corretamente
            entity.Property(v => v.IdVeterinario)
                  .HasColumnName("ID_VETERINARIO")
                  .HasPrecision(10, 0)
                  .ValueGeneratedNever();

            entity.Property(v => v.NomeVeterinario).HasColumnName("NOME_VETERINARIO").IsRequired();
            entity.Property(v => v.CrmVeterinario).HasColumnName("CRM_VETERINARIO").IsRequired();
            entity.Property(v => v.EspecialidadeVeterinario).HasColumnName("ESPECIALIDADE_VETERINARIO").IsRequired();
            entity.Property(v => v.TelefoneVeterinario).HasColumnName("TELEFONE_VETERINARIO");
            entity.Property(v => v.EmailVeterinario).HasColumnName("EMAIL_VETERINARIO");
            entity.Property(v => v.StatusVeterinario).HasColumnName("STATUS_VETERINARIO");
            entity.Property(v => v.DataCadastroVeterinario).HasColumnName("DATA_CADASTRO_VETERINARIO");
            entity.HasIndex(v => v.CrmVeterinario).IsUnique();
            entity.HasIndex(v => v.EmailVeterinario).IsUnique();
        });

        // Mapeamento Tutor
        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.ToTable("TB_TUTOR");
            entity.HasKey(t => t.IdTutor);
            entity.Property(t => t.IdTutor).HasColumnName("ID_TUTOR").HasPrecision(10, 0).ValueGeneratedOnAdd();
            entity.Property(t => t.NomeTutor).HasColumnName("NOME_TUTOR").IsRequired();
            entity.Property(t => t.CpfTutor).HasColumnName("CPF_TUTOR").IsRequired();
            entity.Property(t => t.TelefoneTutor).HasColumnName("TELEFONE_TUTOR");
            entity.Property(t => t.EmailTutor).HasColumnName("EMAIL_TUTOR").IsRequired();
            entity.Property(t => t.CepTutor).HasColumnName("CEP_TUTOR").IsRequired();
            entity.Property(t => t.RuaTutor).HasColumnName("RUA_TUTOR").IsRequired();
            entity.Property(t => t.NumeroTutor).HasColumnName("NUMERO_TUTOR").IsRequired();
            entity.Property(t => t.ComplementoTutor).HasColumnName("COMPLEMENTO_TUTOR");
            entity.Property(t => t.BairroTutor).HasColumnName("BAIRRO_TUTOR").IsRequired();
            entity.Property(t => t.CidadeTutor).HasColumnName("CIDADE_TUTOR").IsRequired();
            entity.Property(t => t.EstadoTutor).HasColumnName("ESTADO_TUTOR").IsRequired();
            entity.Property(t => t.DataCadastroTutor).HasColumnName("DATA_CADASTRO_TUTOR").IsRequired();
        });

        // Mapeamento Pet
        modelBuilder.Entity<Pet>(entity =>
        {
            entity.ToTable("TB_PET");
            entity.HasKey(p => p.IdPet);
            entity.Property(p => p.IdPet).HasColumnName("ID_PET").HasPrecision(10, 0).ValueGeneratedOnAdd();
            entity.Property(p => p.IdTutor).HasColumnName("ID_TUTOR").HasPrecision(10, 0).IsRequired();
            entity.Property(p => p.NomePet).HasColumnName("NOME_PET").IsRequired();
            entity.Property(p => p.EspeciePet).HasColumnName("ESPECIE_PET").IsRequired();
            entity.Property(p => p.RacaPet).HasColumnName("RACA_PET").IsRequired();
            entity.Property(p => p.SexoPet).HasColumnName("SEXO_PET").IsRequired();
            entity.Property(p => p.DataNascimentoPet).HasColumnName("DATA_NASCIMENTO_PET").IsRequired();
            entity.Property(p => p.PesoPet).HasColumnName("PESO_PET").HasPrecision(5, 2).IsRequired();
            entity.Property(p => p.CorPet).HasColumnName("COR_PET").IsRequired();
            entity.Property(p => p.DataCadastroPet).HasColumnName("DATA_CADASTRO_PET").IsRequired();

            entity.HasOne(p => p.Tutor)
                  .WithMany()
                  .HasForeignKey(p => p.IdTutor)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}