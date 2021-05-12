using MediatR;

namespace ImageParser.App.MediatR.Commands
{
    public abstract class CommandBaseWithResponse<TResult> : IRequest<TResult>
    {
    }
}