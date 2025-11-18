using Microsoft.EntityFrameworkCore;
using MantenimientoTrabajadores.Models;

namespace MantenimientoTrabajadores.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Trabajador> Trabajadores { get; set; }
    }
}
