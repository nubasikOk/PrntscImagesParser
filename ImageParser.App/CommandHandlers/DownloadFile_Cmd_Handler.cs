using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ImageParser.App.Commands;
using MediatR;

namespace ImageParser.App.CommandHandlers
{
    public class DownloadFile_Cmd_Handler : IRequestHandler<DownloadFile_Cmd, bool>
    {
        private readonly WebClient _client;
        private bool _isDownloadComplete = true;
        
        public DownloadFile_Cmd_Handler(WebClient client)
        {
            _client = client;
        }

        public async Task<bool> Handle(DownloadFile_Cmd request, CancellationToken cancellationToken)
        {
            
            if(string.IsNullOrEmpty(request.LinkToFile)) throw new ArgumentException(nameof(request.LinkToFile));

            _client.DownloadFileCompleted += CheckDownloadStatus;
            await _client.DownloadFileTaskAsync(new Uri(request.LinkToFile), request.Path);
            
            return _isDownloadComplete;
        }

        private void CheckDownloadStatus(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                _isDownloadComplete = false;
            }
        }
    }
}