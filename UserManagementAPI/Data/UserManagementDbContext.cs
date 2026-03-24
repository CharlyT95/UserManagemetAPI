using Microsoft.EntityFrameworkCore;

namespace UserManagementAPI.Data
{
    public class UserManagementDbContext : DbContext
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        { }
    }
}
