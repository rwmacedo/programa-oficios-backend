using Microsoft.EntityFrameworkCore;
using ProgramaOficios.Application.Entities;
using ProgramaOficios.Application.Interfaces.Repositories;
using ProgramaOficios.Infrastructure.Context;


namespace ProgramaOficios.Infrastructure.Repositories
{
    public class OficioRepository : IOficioRepository
    {
        private readonly OficioDbContext _context;

        public OficioRepository(OficioDbContext context)
        {
            _context = context;
        }

        public async Task<Oficio> GetByIdAsync(int id)
        {
            return await _context.Oficios.FindAsync(id);
        }

        public async Task<IEnumerable<Oficio>> GetAllAsync()
        {
            return await _context.Oficios.ToListAsync();
        }

        public async Task AddAsync(Oficio oficio)
        {
            await _context.Oficios.AddAsync(oficio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Oficio oficio)
        {
            _context.Oficios.Update(oficio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var oficio = await _context.Oficios.FindAsync(id);
            if (oficio != null)
            {
                _context.Oficios.Remove(oficio);
                await _context.SaveChangesAsync();
            }
        }
    }
}
