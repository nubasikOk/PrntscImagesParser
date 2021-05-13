using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ImageStorage.API.Contracts;
using ImageStorage.API.Contracts.ApiResponses;
using ImageStorage.API.MediatR.Commands;
using ImageStorage.API.Options;
using MediatR;

namespace ImageStorage.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly StorageAccountOptions _options;

        public ImagesController(IMediator mediator, StorageAccountOptions options)
        {
            _mediator = mediator;
            _options = options;
        }

        [HttpGet(ApiRoutes.Images.GetAllBlobs)]
        public async Task<IActionResult> GetAllImagesAsync()
        {
            var blobs = await _mediator.Send(new GetAllBlobs_Cmd()
            {
                ContainerName = _options.ContainerName
            });

            return Ok(new GetAllImages_ApiRes()
            {
                Images = blobs.Select(b => new ImageDescription()
                {
                    Name = b.Key,
                    BlobLink = b.Value
                }).ToList()
            });
        }
    }
}
