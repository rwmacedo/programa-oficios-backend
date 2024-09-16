using Microsoft.AspNetCore.Mvc;
using ProgramaOficios.Application.Interfaces.Services;

namespace ProgramaOficios.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;

        public FilesController(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        // Upload de arquivos
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Nenhum arquivo foi enviado.");

            // Garante que o arquivo seja de um formato aceitável (opcional)
            var allowedExtensions = new[] { ".pdf", ".docx", ".jpg", ".png" }; // Exemplo de extensões permitidas
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Tipo de arquivo não suportado.");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                var fileUrl = await _blobStorageService.UploadFileAsync(stream, file.FileName);
                return Ok(new { Url = fileUrl });
            }
        }

        // Download de arquivos
       [HttpGet("download/{fileName}")]
public async Task<IActionResult> DownloadFile(string fileName)
{
    try
    {
        // Baixa o arquivo do Azure Blob Storage
        var fileStream = await _blobStorageService.DownloadFileAsync(fileName);
        if (fileStream == null)
        {
            return NotFound("Arquivo não encontrado.");
        }

        // Define o tipo MIME como application/pdf se o arquivo for um PDF
        var contentType = GetContentType(fileName);

        return File(fileStream, contentType, fileName); // Retorna o arquivo com o tipo correto
    }
    catch (FileNotFoundException)
    {
        return NotFound(new { message = "Arquivo não encontrado no Azure Blob Storage." });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = $"Erro ao fazer o download do arquivo: {ex.Message}" });
    }
}

// Método auxiliar para definir o tipo MIME
private string GetContentType(string fileName)
{
    var extension = Path.GetExtension(fileName).ToLowerInvariant();

    return extension switch
    {
        ".pdf" => "application/pdf", // Tipo MIME para PDF
        ".jpg" => "image/jpeg",      // Exemplo para imagens JPG
        ".png" => "image/png",       // Exemplo para imagens PNG
        _ => "application/octet-stream" // Tipo genérico para outros arquivos
    };
}
    }
}
