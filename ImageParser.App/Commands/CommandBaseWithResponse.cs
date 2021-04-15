using MediatR;

namespace ImageParser.App.Commands
{
    public abstract class CommandBaseWithResponse<TResult> : IRequest<TResult>
    {
    }
}