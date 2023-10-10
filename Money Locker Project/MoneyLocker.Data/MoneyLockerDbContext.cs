using Microsoft.EntityFrameworkCore;
using MoneyLocker.Data.Schema;
using System.Collections.Generic;

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