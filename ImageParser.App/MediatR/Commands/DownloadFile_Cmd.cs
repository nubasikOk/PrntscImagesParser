namespace ImageParser.App.MediatR.Commands
{
    public class DownloadFile_Cmd : CommandBaseWithResponse<byte[]>
    {
        public string LinkToFile { get; set; }
        public string Path { get; set; }
    }
}