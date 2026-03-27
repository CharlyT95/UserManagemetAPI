using Microsoft.EntityFrameworkCore;
using System;
using UserManagementAPI.Models;

namespace UserManagementAPI.Data
{
    public class UserManagementDbContext : DbContext
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Permiso> Permiso { get; set; }
        public DbSet<RolPermiso> RolPermiso { get; set; }
        public DbSet<UsuarioCredencial> UsuariosCredenciale { get; set; }
        public DbSet<UsuarioRol> UsuarioRol {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserManagementDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
