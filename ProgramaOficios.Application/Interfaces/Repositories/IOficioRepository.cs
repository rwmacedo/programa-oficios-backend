using ProgramaOficios.Application.Entities;

namespace ProgramaOficios.Application.Interfaces.Repositories
{
    public interface IOficioRepository
    {
        // Defina os métodos que o repositório de ofícios deve implementar
        Task<Oficio> GetByIdAsync(int id);
        Task<IEnumerable<Oficio>> GetAllAsync();
        Task AddAsync(Oficio oficio);
        Task UpdateAsync(Oficio oficio);
        Task DeleteAsync(int id);
    }
}
