using System;
using System.Threading;
using System.Threading.Tasks;
using ImageParser.App.MediatR.Commands;
using ImageParser.App.Services.Contracts;
using MediatR;

namespace ImageParser.App.MediatR.CommandHandlers
{
    public class GetLinkToFile_Cmd_Handler : IRequestHandler<GetLinkToFile_Cmd, string>
    {
        private readonly IHttpRequestsFactory _httpRequestFactory;
        private readonly IMediator _mediator;

        public GetLinkToFile_Cmd_Handler(IHttpRequestsFactory httpRequestFactory, IMediator mediator)
        {
            _httpRequestFactory = httpRequestFactory;
            _mediator = mediator;
        }

        public async Task<string> Handle(GetLinkToFile_Cmd request, CancellationToken cancellationToken)
        {
            var responseForParsing = await _httpRequestFactory.GetHttpRequestTask();

            if (!responseForParsing.IsSuccessful) throw new Exception("Http request to the server failed!");

            var parsedLink = await _mediator.Send(new ParseLinkFromResponse_Cmd
            {
                ResponseContent = responseForParsing.Content
            }, cancellationToken);

            return parsedLink;
        }
    }
}