using ProgramaOficios.Application.Dto;

namespace ProgramaOficios.Application.Interfaces.Services
{
    public interface IOficioService
    {
        Task<OficioDto> GetByIdAsync(int id);
        Task<IEnumerable<OficioDto>> GetAllAsync();
        Task AddAsync(OficioDto oficio);
        Task UpdateAsync(OficioDto oficio);
        Task DeleteAsync(int id);
    }
}
