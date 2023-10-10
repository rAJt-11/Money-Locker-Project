using Microsoft.EntityFrameworkCore;
using MoneyLocker.Data.Schema;

namespace MoneyLocker.Data
{
    public class MoneyLockerDbContext : DbContext
    {
        public MoneyLockerDbContext(DbContextOptions<MoneyLockerDbContext> options) : base(options)
        {
        }

        public DbSet<UserInfo> UserInfo { get; set; }
    }
}