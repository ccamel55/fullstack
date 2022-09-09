using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Calculation>()
                .HasKey(p => p.timeStamp);
        }

        public DbSet<Calculation> Calculations => Set<Calculation>();
    }
}
