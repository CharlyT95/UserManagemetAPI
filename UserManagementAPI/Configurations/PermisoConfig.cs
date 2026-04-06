using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementAPI.Models;

namespace UserManagementAPI.Configurations
{
    public class PermisoConfig : IEntityTypeConfiguration<Permiso>
    {
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.HasKey(x => x.IdPermiso);

            builder.HasIndex(x => x.CodigoPermiso).IsUnique();

            builder.Property(x => x.CodigoPermiso)
                   .IsRequired()
                   .HasMaxLength(50);
        }
    }
}
