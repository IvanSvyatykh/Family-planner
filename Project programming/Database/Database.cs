using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project_programming.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Family> Families { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=;user=;password=;database=;",
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}
