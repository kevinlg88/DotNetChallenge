using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Data
{
    public class AppDbContext: DbContext
    {
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> option) : base (option)
        {
        }

    }
}
