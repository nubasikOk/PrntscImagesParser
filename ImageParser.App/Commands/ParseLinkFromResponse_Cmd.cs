namespace ImageParser.App.Commands
{
    public class ParseLinkFromResponse_Cmd : CommandBaseWithResponse<string>
    {
        public string ResponseContent { get; set; }
    }
}