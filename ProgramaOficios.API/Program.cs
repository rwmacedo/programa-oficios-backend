using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using dotenv.net;
using ProgramaOficios.Infrastructure.Context;
using ProgramaOficios.Infrastructure.IoC;



var builder = WebApplication.CreateBuilder(args);

// Carregar variáveis do arquivo .env
DotEnv.Load();

Console.WriteLine($"SQL_CONNECTION_STRING: {Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING")}");
Console.WriteLine($"AZURE_BLOB_STORAGE: {Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE")}");

// Adicionar serviços antes do Build
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.ConfigurarInjecaoDeDependencia();

// Registrar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")  // Origem do Angular
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Pegar strings de conexão do .env
var sqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") 
    ?? throw new InvalidOperationException("A string de conexão SQL não foi configurada corretamente.");
var blobStorageConnectionString = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE") 
    ?? throw new InvalidOperationException("A conexão com o Azure Blob Storage não foi configurada corretamente.");

// Configurar o DbContext para o SQL Server
builder.Services.AddDbContext<OficioDbContext>(options =>
    options.UseSqlServer(sqlConnectionString));

// Configurar o BlobServiceClient usando o Configuration diretamente
builder.Services.AddSingleton(new BlobServiceClient(blobStorageConnectionString));

var app = builder.Build(); // Build ocorre aqui

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

// Configuração do middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Usar a política de CORS correta ("AllowAngularClient")
app.UseCors("AllowAngularClient");

app.MapControllers();

app.Run();
