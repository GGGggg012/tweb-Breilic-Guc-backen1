using eAviaSales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eAviaSales.DataAccess.Context
{
    public class OrderContext : DbContext
    {
        public DbSet<OrderData> Orders { get; set; }
        public DbSet<OrderItemData> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(DbSession.ConnectionString,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory_Order"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderData>()
                .HasMany(o => o.Items)
                .WithOne(i => i.Order)
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
