using System.Text.Json;
using Azure.Storage.Blobs;
using GCGRA.UPDB.Core.Entities;
using GCGRA.UPDB.Core.Interfaces;

namespace GCGRA.UPDB.Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public BlobStorageService(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;
        }

        public async Task<string> UploadJsonToBlobAsync(object data, string blobName)
        {
            // Get the current date to create a folder path (e.g., "2025/04/06")
            var dateFolder = DateTime.UtcNow.ToString("yyyyMMdd");

            // Combine the folder path with the blob name
            var fullBlobName = $"{dateFolder}/{blobName}";

            // Initialize the BlobServiceClient and get the container client
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);

            // Ensure the container exists
            await blobContainerClient.CreateIfNotExistsAsync();

            // Get the blob client for the combined path
            var blobClient = blobContainerClient.GetBlobClient(fullBlobName);

            // Serialize the data to JSON
            var json = JsonSerializer.Serialize(data);

            // Upload the JSON data to the blob
            using (var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(json)))
            {
                await blobClient.UploadAsync(stream, overwrite: true); // Overwrite if the blob exists
            }

            // Return the URI of the uploaded blob
            return blobClient.Uri.ToString();
        }        
    }
}