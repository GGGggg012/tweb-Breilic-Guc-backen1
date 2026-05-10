using eAviaSales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eAviaSales.DataAccess.Context
{
    public class UserContext : DbContext
    {
        public DbSet<UserData> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(DbSession.ConnectionString,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory_User"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserData>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
