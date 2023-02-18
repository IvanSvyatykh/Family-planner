using Microsoft.EntityFrameworkCore;
using Project_programming.Database;

namespace MySQLApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=s518530;password=XVCeY7nR;database=s518530_Family_Planner;",
                new MySqlServerVersion(new Version(8, 0, 11))
            );
        }
    }
}
