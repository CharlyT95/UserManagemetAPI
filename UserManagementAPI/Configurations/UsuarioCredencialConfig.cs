using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagementAPI.Models;

namespace UserManagementAPI.Configurations
{
    public class UsuarioCredencialConfig : IEntityTypeConfiguration<UsuarioCredencial>
    {
        public void Configure(EntityTypeBuilder<UsuarioCredencial> builder)
        {
            builder.HasKey(x => x.IdUsuarioCredencial);
        }
    }
}
