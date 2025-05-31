using Entities.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infra.Configuracao
{
    public class ContextBase : IdentityDbContext<ApplicationUser>
    {
        public ContextBase(DbContextOptions options) : base(options) { }

        public DbSet<SistemaFinanceiro> SistemaFinanceiro { get; set; }
        public DbSet<UsuarioSistemaFinanceiro> UsuarioSistemaFinanceiro { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Despesa> Despesa { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(ObterStringConexao());
        //        base.OnConfiguring(optionsBuilder);
        //    }
            
        //}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuração do Identity
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

            // Configurações explícitas para cada entidade
            builder.Entity<SistemaFinanceiro>(entity =>
            {
                entity.ToTable("SistemaFinanceiro");
                entity.HasKey(s => s.SistemaFinanceiroID);
            });

            builder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categoria");
                entity.HasKey(c => c.CategoriaID);
                entity.HasOne(c => c.SistemaFinanceiro)
                      .WithMany()
                      .HasForeignKey(c => c.SistemaID);
            });

            builder.Entity<Despesa>(entity =>
            {
                entity.ToTable("Despesa");
                entity.HasKey(d => d.DespesaID);
                entity.HasOne(d => d.Categoria)
                      .WithMany()
                      .HasForeignKey(d => d.CategoriaID);
            });

            builder.Entity<UsuarioSistemaFinanceiro>(entity =>
            {
                entity.ToTable("UsuarioSistemaFinanceiro");
                entity.HasKey(u => u.UsuarioSistemaFinanceiroID);
                entity.HasOne(u => u.SistemaFinanceiro)
                      .WithMany()
                      .HasForeignKey(u => u.SistemaID);
            });
        }
        public string ObterStringConexao()
        {
            return @"Data Source=JHONPC\SQLEXPRESS;Initial Catalog=FN2025;Integrated Security=True;TrustServerCertificate=True";
        }
    }
}
