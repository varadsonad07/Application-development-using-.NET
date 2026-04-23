using Microsoft.EntityFrameworkCore;
using studenttCrudapp.Models;

namespace studenttCrudapp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=StudentDB;Trusted_Connection=True;");
        }
    }
}
