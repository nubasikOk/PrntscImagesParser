using System.Collections.Generic;

namespace ImageStorage.API.MediatR.Commands
{
    public class GetAllBlobs_Cmd : CommandBaseWithResponse<Dictionary<string,string>>
    {
        public string ContainerName { get; set; }
    }
}