using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using ProgramaOficios.Application.Interfaces.Services;

namespace ProgramaOficios.Infrastructure.Services
{
    public class BlobStorageInfraService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageInfraService(IConfiguration configuration, BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _containerName = configuration["AzureBlobStorage:ContainerName"] ?? throw new ArgumentNullException(nameof(_containerName));
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);

            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadFileAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                return await blobClient.OpenReadAsync();
            }
            throw new FileNotFoundException("Arquivo não encontrado no Azure Blob Storage.");
        }
    }
}
