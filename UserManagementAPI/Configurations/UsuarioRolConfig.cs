using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementAPI.Models;

namespace UserManagementAPI.Configurations
{
    public class UsuarioRolConfig : IEntityTypeConfiguration<UsuarioRol>
    {
        public void Configure(EntityTypeBuilder<UsuarioRol> builder)
        {
            builder.HasKey(x => x.IdUsuario);
        }
    }
}
