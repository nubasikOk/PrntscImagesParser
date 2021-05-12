namespace ImageParser.App.MediatR.Commands
{
    public class ParseLinkFromResponse_Cmd : CommandBaseWithResponse<string>
    {
        public string ResponseContent { get; set; }
    }
}