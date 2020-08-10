using SSSA.Core.Api.Communication.Mediator;

namespace SSSA.Core.Api.Communication.Commands
{
    public abstract class CommandHandlerBase
    {
        protected IMediatorHandler MediatorHandler { get; }

        protected CommandHandlerBase(
            IMediatorHandler mediatorHandler)
        {
            MediatorHandler = mediatorHandler;
        }
    }
}
