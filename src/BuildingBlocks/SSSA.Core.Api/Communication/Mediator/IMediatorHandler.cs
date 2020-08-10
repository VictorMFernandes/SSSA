using SSSA.Core.Api.Communication.Commands;
using SSSA.Core.Api.Communication.Errors;
using System.Threading.Tasks;

namespace SSSA.Core.Api.Communication.Mediator
{
    public interface IMediatorHandler
    {
        Task<TResult> SendCommandAsync<TResult>(CommandBase<TResult> command);
        Task NotifyErrorAsync<T>(T error)
            where T : ErrorNotification;
    }
}