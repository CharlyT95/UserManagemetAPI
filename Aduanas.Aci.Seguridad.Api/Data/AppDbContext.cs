using Aduanas.Aci.Seguridad.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Aduanas.Aci.Seguridad.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario>            Usuario            => Set<Usuario>();
    public DbSet<UsuarioCredencial>  UsuarioCredencial => Set<UsuarioCredencial>();
    public DbSet<UsuarioRol>         UsuarioRol        => Set<UsuarioRol>();
    public DbSet<Rol>                Rol               => Set<Rol>();
    public DbSet<Permiso>            Permiso            => Set<Permiso>();
    public DbSet<RolPermiso>         RolPermiso         => Set<RolPermiso>();
    public DbSet<RefreshToken>       RefreshToken       => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // UsuarioRol
        modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.Usuario)
            .WithMany()
            .HasForeignKey(ur => ur.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UsuarioRol>()
            .HasOne(ur => ur.Rol)
            .WithMany()
            .HasForeignKey(ur => ur.IdRol)
            .OnDelete(DeleteBehavior.Restrict);

        // RolPermiso
        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Rol)
            .WithMany()
            .HasForeignKey(rp => rp.IdRol)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RolPermiso>()
            .HasOne(rp => rp.Permiso)
            .WithMany()
            .HasForeignKey(rp => rp.IdPermiso)
            .OnDelete(DeleteBehavior.Restrict);

        // UsuarioCredencial
        modelBuilder.Entity<UsuarioCredencial>()
            .HasOne(uc => uc.Usuario)
            .WithMany()
            .HasForeignKey(uc => uc.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // RefreshToken
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.Usuario)
            .WithMany()
            .HasForeignKey(rt => rt.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        // Índice en Token para búsquedas rápidas
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(rt => rt.Token)
            .IsUnique();

        modelBuilder.Entity<RefreshToken>()
            .ToTable("RefreshToken")  // ← nombre exacto de la BD
            .HasOne(rt => rt.Usuario)
            .WithMany()
            .HasForeignKey(rt => rt.IdUsuario)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasIndex(rt => rt.Token)
            .IsUnique();
    }
}
