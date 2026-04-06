using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementAPI.Models;

namespace UserManagementAPI.Configurations
{
    public class UsuarioConfig : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(x => x.IdUsuario);
            builder.HasIndex(x => x.CorreoElectronico).IsUnique();
            builder.HasIndex(x => x.UsuarioLogin).IsUnique();

        }
    }
}
