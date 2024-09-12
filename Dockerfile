# Use uma imagem base do .NET SDK para construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar os arquivos de projeto corretos
COPY ProgramaOficios.API/ProgramaOficios.API.csproj ./ProgramaOficios.API/
COPY ProgramaOficios.Application/ProgramaOficios.Application.csproj ./ProgramaOficios.Application/
COPY ProgramaOficios.Infrastructure/ProgramaOficios.Infrastructure.csproj ./ProgramaOficios.Infrastructure/

# Restaure as dependências
RUN dotnet restore ./ProgramaOficios.API/ProgramaOficios.API.csproj

# Copie o restante dos arquivos de código
COPY . .

# Compile e publique o aplicativo
RUN dotnet publish ./ProgramaOficios.API/ProgramaOficios.API.csproj -c Release -o /out

# Use uma imagem base mais leve para o runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /out .

# Exponha a porta 80
EXPOSE 80

# Comando de entrada para rodar o aplicativo
ENTRYPOINT ["dotnet", "ProgramaOficios.API.dll"]
