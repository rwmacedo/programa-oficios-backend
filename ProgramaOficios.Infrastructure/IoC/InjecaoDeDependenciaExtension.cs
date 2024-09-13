using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProgramaOficios.Application.Interfaces.Repositories;
using ProgramaOficios.Application.Interfaces.Services;
using ProgramaOficios.Application.Services;
using ProgramaOficios.Infrastructure.Repositories;
using ProgramaOficios.Infrastructure.Services;

namespace ProgramaOficios.Infrastructure.IoC
{
    public static class InjecaodeDependenciaExtension
    {
        public static WebApplicationBuilder ConfigurarInjecaoDeDependencia(this WebApplicationBuilder builder)
        {
            // Registrar o BlobStorageService para injeção de dependência
            builder.Services.AddScoped<IBlobStorageService, BlobStorageInfraService>();
            builder.Services.AddScoped<IOficioRepository, OficioRepository>();
            builder.Services.AddScoped<IOficioService, OficioService>();

            return builder;
        }
    }
}
