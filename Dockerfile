# Use uma imagem base do .NET SDK para construir a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copiar os arquivos de projeto corretos
COPY ProgramaOficios.API*.csproj ./ProgramaOficios.API/

# Restaure as dependências
RUN dotnet restore ./ProgramaOficios.API/*.csproj

# Copie o restante dos arquivos de código
COPY . .

# Compile e publique o aplicativo
RUN dotnet publish ./ProgramaOficios.API/*.csproj -c Release -o /out

# Use uma imagem base mais leve para o runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build-env /out .

# Pegue a porta dinâmica do Heroku
ENV ASPNETCORE_URLS=http://+:${PORT}

# Exponha a porta 80 para o localhost (opcional)
EXPOSE 80

# Comando de entrada para rodar o aplicativo
ENTRYPOINT ["dotnet", "ProgramaOficios.API.dll"]
