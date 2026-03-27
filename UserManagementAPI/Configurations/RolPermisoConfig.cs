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
        }
    }
}
