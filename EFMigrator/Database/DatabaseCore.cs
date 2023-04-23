using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Classes;

namespace Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<GoodsCategory> Goods { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Family>().HasKey(f=> f.Id); 
            modelBuilder.Entity<GoodsCategory>().HasKey(g=> g.Id);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=raja.db.elephantsql.com;Port=5432;Database=rdadzjjb;Username=rdadzjjb;Password=MYoIJWiErDHEkUBEKQ5oUOzQfpNgX_x4");
        }
    }
}
