using MediatR;

namespace ImageStorage.API.MediatR.Commands
{
    public abstract class CommandBaseWithResponse<TResult> : IRequest<TResult>
    {
    }
}