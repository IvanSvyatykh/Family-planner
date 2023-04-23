using Microsoft.EntityFrameworkCore;
using Classes;

namespace Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Family> Families { get; set; }   

        public DbSet<GoodsCategory> Goods { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=raja.db.elephantsql.com;Port=5432;Database=rdadzjjb;Username=rdadzjjb;Password=MYoIJWiErDHEkUBEKQ5oUOzQfpNgX_x4");
        }
    }
}
