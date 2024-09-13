using ProgramaOficios.Application.Dto;
using ProgramaOficios.Application.Entities;
using ProgramaOficios.Application.Interfaces.Repositories;
using ProgramaOficios.Application.Interfaces.Services;

namespace ProgramaOficios.Application.Services
{
    public class OficioService : IOficioService
    {
        private readonly IOficioRepository _oficioRepository;

        public OficioService(IOficioRepository oficioRepository)
        {
            _oficioRepository = oficioRepository;
        }

        public async Task<OficioDto> GetByIdAsync(int id)
        {
            var oficio = await _oficioRepository.GetByIdAsync(id);

            if (oficio == null)
            {
                throw new Exception("Ofício não encontrado.");
            }

            return MapToDto(oficio); // Converte a entidade para DTO
        }

        public async Task<IEnumerable<OficioDto>> GetAllAsync()
        {
            var oficios = await _oficioRepository.GetAllAsync();
            return oficios.Select(oficio => MapToDto(oficio));
        }

        // Método para adicionar um novo Ofício
        public async Task AddAsync(OficioDto oficioDto)
        {
            var oficio = MapToEntity(oficioDto); // Converte o DTO para entidade
            await _oficioRepository.AddAsync(oficio);
        }

        // Método para atualizar um Ofício existente
        public async Task UpdateAsync(OficioDto oficioDto)
        {
            var oficio = MapToEntity(oficioDto);
            await _oficioRepository.UpdateAsync(oficio);
        }

        // Método para excluir um Ofício
        public async Task DeleteAsync(int id)
        {
            await _oficioRepository.DeleteAsync(id);
        }

        // Métodos auxiliares para mapear entre DTO e entidade (Oficio)
        private OficioDto MapToDto(Oficio oficio)
        {
            return new OficioDto
            {
                Id = oficio.Id,
                Numero = oficio.Numero,
                Ano = oficio.Ano,
                Unidade = oficio.Unidade,
                Data = oficio.Data,
                ArquivoUrl = oficio.ArquivoUrl // Mapeando o campo ArquivoUrl
            };
        }

        private Oficio MapToEntity(OficioDto dto)
        {
            // Utiliza o construtor parametrizado de Oficio
            return new Oficio(dto.Numero, dto.Ano, dto.Unidade, dto.Data)
            {
                Id = dto.Id,  // Inclui o Id se for necessário
                ArquivoUrl = dto.ArquivoUrl // Mapeia o ArquivoUrl
            };
        }
    }
}
