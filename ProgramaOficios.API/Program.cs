using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using ProgramaOficios.Infrastructure.Context;
using ProgramaOficios.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

// Exibir as variáveis de ambiente para depuração
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
    options.AddPolicy("AllowHerokuClient",
        builder =>
        {
            builder.WithOrigins("https://*.controle-oficios-frontend-2add7ce34be5.herokuapp.com")  // Origem do Angular
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Pegar strings de conexão do ambiente
var sqlConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING") 
    ?? throw new InvalidOperationException("A string de conexão SQL não foi configurada corretamente.");
var blobStorageConnectionString = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE") 
    ?? throw new InvalidOperationException("A conexão com o Azure Blob Storage não foi configurada corretamente.");

// Configurar o DbContext para o PostgreSQL
builder.Services.AddDbContext<OficioDbContext>(options =>
    options.UseNpgsql(sqlConnectionString));

// Configurar o BlobServiceClient usando o Configuration diretamente
builder.Services.AddSingleton(new BlobServiceClient(blobStorageConnectionString));

var app = builder.Build(); 

// Aplicar migrations automaticamente
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OficioDbContext>();
    dbContext.Database.Migrate();  // Aplica as migrations
}

// Pegue a porta do ambiente no Heroku
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://*:{port}");

// Adicionar logs para ver o caminho da requisição
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request for {context.Request.Path} received.");
    await next.Invoke();
});

// Configuração do middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar a política de CORS correta ("AllowHerokuClient")
app.UseCors("AllowHerokuClient");

app.UseAuthorization();

// Usar o Swagger na produção também
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    c.RoutePrefix = string.Empty;  // Define o Swagger na raiz
});

app.MapControllers();

app.Run();
