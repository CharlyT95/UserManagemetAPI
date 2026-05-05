using Aduanas.Aci.Audit.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aduanas.Aci.Audit.Api.Data
{
    public class AuditoriaDbContext : DbContext
    {
        public AuditoriaDbContext(DbContextOptions<AuditoriaDbContext> options)
            : base(options) { }

        public DbSet<AuditoriaLog> AuditoriaLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditoriaLog>(entity =>
            {
                entity.ToTable("AuditoriaLog");
                entity.HasKey(e => e.IdLog);
                entity.Property(e => e.IdLog).UseIdentityColumn();
                entity.Property(e => e.ValorAnterior).HasColumnType("nvarchar(max)");
                entity.Property(e => e.ValorNuevo).HasColumnType("nvarchar(max)");
                entity.Property(e => e.FechaEvento).HasColumnType("datetime2");
            });
        }
    }
}
