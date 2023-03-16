using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Classes;
using Families;

namespace Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Family> Families { get; set; } = null!;
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Family>().HasKey(f=> f.Id); 
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=raja.db.elephantsql.com;Port=5432;Database=rdadzjjb;Username=rdadzjjb;Password=MYoIJWiErDHEkUBEKQ5oUOzQfpNgX_x4");
        }
    }
}
