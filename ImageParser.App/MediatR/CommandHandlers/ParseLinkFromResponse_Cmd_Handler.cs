using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;
using ImageParser.App.MediatR.Commands;
using MediatR;

namespace ImageParser.App.MediatR.CommandHandlers
{
    public class ParseLinkFromResponse_Cmd_Handler : IRequestHandler<ParseLinkFromResponse_Cmd, string>
    {
        private readonly IHtmlParser _htmlParser;

        public ParseLinkFromResponse_Cmd_Handler(IHtmlParser htmlParser)
        {
            _htmlParser = htmlParser;
        }

        public Task<string> Handle(ParseLinkFromResponse_Cmd command, CancellationToken cancellationToken)
        {
            var document = _htmlParser.ParseDocument(command.ResponseContent);
            var tags = document.QuerySelectorAll("meta");

            var result = string.Empty;
            foreach (var element in tags)
            {
                var content = element.GetAttribute("content");
                if (content != null && (content.Contains("https://i.imgur.com") ||
                                        content.Contains("https://image.prntscr.com/")))
                {
                    result = content;
                    break;
                }
            }

            return Task.FromResult(result);
        }
    }
}