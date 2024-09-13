using Microsoft.EntityFrameworkCore;
using ProgramaOficios.Application.Entities;

namespace ProgramaOficios.Infrastructure.Context
{
    public class OficioDbContext : DbContext
    {
        public OficioDbContext(DbContextOptions<OficioDbContext> options)
            : base(options)
        {
        }

        public DbSet<Oficio> Oficios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurar o mapeamento da entidade Oficio, se necessário
            base.OnModelCreating(modelBuilder);
        }
    }
}
