using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GastosPersonales.Models;

namespace GastosPersonales.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GastosPersonales.Models.Categoria> Categoria { get; set; } = default!;
        public DbSet<GastosPersonales.Models.SaldoMensual> SaldoMensual { get; set; } = default!;
        public DbSet<GastosPersonales.Models.Comprobante> Comprobante { get; set; } = default!;
    }
}