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
                var fileStream = await _blobStorageService.DownloadFileAsync(fileName);
                if (fileStream == null)
                {
                    return NotFound("Arquivo não encontrado.");
                }

                return File(fileStream, "application/octet-stream", fileName);
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
    }
}
