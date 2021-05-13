using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageStorage.API.Options;
using ImageStorage.API.Services.Contracts;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace ImageStorage.API.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly CloudBlobClient _client;

        public BlobStorageService(StorageAccountOptions options)
        {
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

        public async IAsyncEnumerable<CloudBlob> ListBlobsFlatListingAsync(string containerName, int? segmentSize)
        {
            var container = GetContainerReference(containerName);
            BlobContinuationToken continuationToken = null;
            
            do
            {
                var resultSegment = await container.ListBlobsSegmentedAsync(
                    string.Empty,
                    true, 
                    BlobListingDetails.Metadata, 
                    segmentSize,
                    continuationToken, 
                    null,
                    null);

                foreach (var blobItem in resultSegment.Results)
                {
                    yield return  blobItem as CloudBlob;
                }

                continuationToken = resultSegment.ContinuationToken;
            } while (continuationToken != null);
        }
    }
}