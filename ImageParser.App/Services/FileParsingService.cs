using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageParser.App.MediatR.Commands;
using ImageParser.App.MediatR.Events;
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
                    var fileName = $"{ Guid.NewGuid() }.png";
                    var pathToSave = Path.Combine(Program.Configuration.GetValue<string>("GrabbedFilesFolder"),fileName);
                    if (!string.IsNullOrEmpty(linkToFile))
                    {
                        _logger.Info($"Link to file {linkToFile}. Starting downloading to local path {pathToSave}");
                        var downloadResult = await _mediator.Send(new DownloadFile_Cmd()
                        {
                            LinkToFile = linkToFile,
                            Path = pathToSave
                        }, stoppingToken);

                        await _mediator.Publish(new ImageFileDownloaded_Event()
                        {
                            BlobName = fileName,
                            FileContentBytes = downloadResult
                        }, stoppingToken);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error($"Something went wrong: {e.Message}");
                }
            } while (!stoppingToken.IsCancellationRequested);
        }
    }
}