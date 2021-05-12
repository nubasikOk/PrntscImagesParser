using System.Threading;
using System.Threading.Tasks;
using ImageParser.App.MediatR.Events;
using ImageParser.App.Options;
using ImageParser.App.Services.Contracts;
using MediatR;
using NLog;

namespace ImageParser.App.MediatR.EventHandlers
{
    public class UploadFileToBlob_Cmd_Handler : INotificationHandler<ImageFileDownloaded_Event>
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly StorageAccountOptions _storageAccountOptions;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public UploadFileToBlob_Cmd_Handler(IBlobStorageService blobStorageService, StorageAccountOptions storageAccountOptions)
        {
            _blobStorageService = blobStorageService;
            _storageAccountOptions = storageAccountOptions;
        }
        
        public async Task Handle(ImageFileDownloaded_Event notification, CancellationToken cancellationToken)
        {
            await _blobStorageService.UploadByteFileToBlobAsync(notification.FileContentBytes, notification.BlobName, _storageAccountOptions.ContainerName);

            _logger.Info($"File {notification.BlobName} successfully uploaded into blob storage. Link to download {_blobStorageService.GetDownloadLinkWithSasToken(notification.BlobName, _storageAccountOptions.ContainerName)}");
        }
    }
}