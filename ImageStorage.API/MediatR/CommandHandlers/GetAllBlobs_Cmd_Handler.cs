using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageStorage.API.MediatR.Commands;
using ImageStorage.API.Options;
using ImageStorage.API.Services.Contracts;
using MediatR;

namespace ImageStorage.API.MediatR.CommandHandlers
{
    public class GetAllBlobs_Cmd_Handler : IRequestHandler<GetAllBlobs_Cmd, Dictionary<string, string>>
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly StorageAccountOptions _options;

        public GetAllBlobs_Cmd_Handler(IBlobStorageService blobStorageService, StorageAccountOptions options)
        {
            _blobStorageService = blobStorageService;
            _options = options;
        }

        public async Task<Dictionary<string, string>> Handle(GetAllBlobs_Cmd request, CancellationToken cancellationToken)
        {
            var result = new Dictionary<string, string>();

            await foreach (var blob in _blobStorageService.ListBlobsFlatListingAsync(request.ContainerName, null).WithCancellation(cancellationToken))
            {
                result.Add(blob.Name, _blobStorageService.GetDownloadLinkWithSasToken(blob.Name, _options.ContainerName));
            }

            return result;
        }
    }
}