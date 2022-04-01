using Microsoft.EntityFrameworkCore;

namespace HealthPass.Data.Entities
{
    public class HealthPassDataContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<LoginDetails> LoginDetails { get; set; }
        public virtual DbSet<BlockedSignature> BlockedSignatures { get; set; }

        public string DbPath { get; }

        public HealthPassDataContext()
        {
            var path = Environment.CurrentDirectory;
            DbPath = Path.Join(path, "healthpass.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }
}
