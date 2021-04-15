namespace ImageParser.App.Commands
{
    public class DownloadFile_Cmd : CommandBaseWithResponse<bool>
    {
        public string LinkToFile { get; set; }
        public string Path { get; set; }
    }
}