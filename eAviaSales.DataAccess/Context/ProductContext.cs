using eAviaSales.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eAviaSales.DataAccess.Context
{
    public class ProductContext : DbContext
    {
        public DbSet<ProductData> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(DbSession.ConnectionString,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory_Product"));
        }
    }
}
