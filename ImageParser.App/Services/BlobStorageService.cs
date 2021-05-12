using System;
using System.Threading.Tasks;
using ImageParser.App.Options;
using ImageParser.App.Services.Contracts;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace ImageParser.App.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly CloudBlobClient _client;
        private readonly StorageAccountOptions _options;

        public BlobStorageService(StorageAccountOptions options)
        {
            _options = options;
            _client = CloudStorageAccount.Parse(options.ConnectionString)
                .CreateCloudBlobClient();
        }

        public async Task UploadByteFileToBlobAsync(byte[] content, string blobPath, string containerName)
        {
            var container = GetContainerReference(containerName);

            var reference = container.GetBlockBlobReference(blobPath);
            await reference.UploadFromByteArrayAsync(content, 0, content.Length);
        }

        public async Task UploadTextFileToBlobAsync(string content, string blobPath, string containerName)
        {
            var container = GetContainerReference(containerName);

            var reference = container.GetBlockBlobReference(blobPath);
            await reference.UploadTextAsync(content);
        }

        public async Task UploadLocalFileToBlobAsync(string path, string blobPath, string containerName)
        {
            var container = GetContainerReference(containerName);

            var reference = container.GetBlockBlobReference(blobPath);
            await reference.UploadFromFileAsync(path);
        }

        public async Task<byte[]> DownloadFileFromBlobAsync(string path, string containerName)
        {
            var container = GetContainerReference(containerName);
            var reference = container.GetBlockBlobReference(path);

            await reference.FetchAttributesAsync();
            var content = new byte[reference.Properties.Length];

            await reference.DownloadToByteArrayAsync(content, 0);

            return content;
        }

        public async Task<string> DownloadTextFileFromBlobAsync(string path, string containerName)
        {
            var container = GetContainerReference(containerName);
            var reference = container.GetBlockBlobReference(path);

            return await reference.DownloadTextAsync();
        }
        public string GetDownloadLinkWithSasToken(string blobName, string containerName)
        {
            var blob = GetCloudBlob(blobName, containerName);

            // Create a new access policy for the account.
            var policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24)
            };

            return $"{blob.Uri}{blob.GetSharedAccessSignature(policy)}";
        }

        private CloudBlockBlob GetCloudBlob(string blobName, string containerName)
        {
            return _client
                .GetContainerReference(containerName)
                .GetBlockBlobReference(blobName);
        }

        private CloudBlobContainer GetContainerReference(string containerName)
        {
            var container = _client.GetContainerReference(containerName);
            container.CreateIfNotExists();

            return container;
        }
    }
}