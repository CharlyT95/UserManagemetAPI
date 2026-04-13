using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementAPI.Models;

namespace UserManagementAPI.Configurations
{
    public class RolPermisoConfig : IEntityTypeConfiguration<RolPermiso>
    {
        public void Configure(EntityTypeBuilder<RolPermiso> builder)
        {
            builder.HasKey(x => x.IdRolPermiso);

            builder.HasOne(e => e.Rol)
                .WithMany()
                .HasForeignKey(e => e.IdRol)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Permiso)
                .WithMany()
                .HasForeignKey(e => e.IdPermiso)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
