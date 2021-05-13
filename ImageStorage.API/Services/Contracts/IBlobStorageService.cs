using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;

namespace ImageStorage.API.Services.Contracts
{
    public interface IBlobStorageService
    {
        Task UploadByteFileToBlobAsync(byte[] content, string blobPath, string containerName);

        Task UploadTextFileToBlobAsync(string content, string blobPath, string containerName);

        Task UploadLocalFileToBlobAsync(string path, string blobPath, string containerName);

        Task<byte[]> DownloadFileFromBlobAsync(string path, string containerName);

        Task<string> DownloadTextFileFromBlobAsync(string path, string containerName);

        IAsyncEnumerable<CloudBlob> ListBlobsFlatListingAsync(string containerName, int? segmentSize);

        string GetDownloadLinkWithSasToken(string blobName, string containerName);
    }
}