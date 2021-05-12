using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ImageParser.App.MediatR.Commands;
using MediatR;

namespace ImageParser.App.MediatR.CommandHandlers
{
    public class DownloadFile_Cmd_Handler : IRequestHandler<DownloadFile_Cmd, byte[]>
    {
        private readonly WebClient _client;
        
        public DownloadFile_Cmd_Handler(WebClient client)
        {
            _client = client;
        }

        public async Task<byte[]> Handle(DownloadFile_Cmd request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.LinkToFile)) throw new ArgumentException(nameof(request.LinkToFile));

            await _client.DownloadFileTaskAsync(new Uri(request.LinkToFile), request.Path);
            
            return await File.ReadAllBytesAsync(request.Path, cancellationToken);
        }
    }
}