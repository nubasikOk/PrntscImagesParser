using MediatR;

namespace ImageParser.App.MediatR.Events
{
    public class ImageFileDownloaded_Event : INotification
    {
        public string BlobName { get; set; }

        public byte[] FileContentBytes { get; set; }
    }
}