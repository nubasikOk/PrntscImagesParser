using System.Collections.Generic;

namespace ImageStorage.API.Contracts.ApiResponses
{
    public class GetAllImages_ApiRes
    {
        public List<ImageDescription> Images { get; set; }
    }

    public class ImageDescription
    {
        public string Name { get; set; }

        public string BlobLink { get; set; }
    }
}