using System;
using System.Threading;
using System.Threading.Tasks;
using ImageParser.App.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;

namespace ImageParser.App.Services
{
    public class FileParsingService : BackgroundService
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly IMediator _mediator;
       
        public FileParsingService(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                try
                {
                    var linkToFile = await _mediator.Send(new GetLinkToFile_Cmd(), stoppingToken);
                    var downloadPath = $@"{Program.Configuration.GetValue<string>("GrabbedFilesFolder")}\{Guid.NewGuid()}.png";
                    if (!string.IsNullOrEmpty(linkToFile))
                    {
                        _logger.Info($"Link to file {linkToFile}. Starting downloading to local path {downloadPath}");
                        var downloadResult = await _mediator.Send(new DownloadFile_Cmd()
                        {
                            LinkToFile = linkToFile,
                            Path = downloadPath
                        }, stoppingToken);
                        _logger.Info($"File downloaded.Result: {downloadResult}");
                    }

                    await Task.Delay(2000, stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.Error($"Something went wrong: {e.Message}");
                }
            } while (!stoppingToken.IsCancellationRequested);
        }
    }
}