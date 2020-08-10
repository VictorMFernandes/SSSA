using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using SSSA.Core.Api.Communication.Commands;
using SSSA.Core.Api.Communication.Errors;
using System.Threading.Tasks;

namespace SSSA.Core.Api.Communication.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MediatorHandler> _logger;
        private readonly IStringLocalizer<MediatorHandler> _localizer;

        public MediatorHandler(
            IMediator mediator,
            ILogger<MediatorHandler> logger,
            IStringLocalizer<MediatorHandler> localizer)
        {
            _mediator = mediator;
            _logger = logger;
            _localizer = localizer;
        }

        public async Task<TResult> SendCommandAsync<TResult>(CommandBase<TResult> command)
        {
            _logger.LogInformation(_localizer["Command {@command} sent."], command);
            var result = await _mediator.Send(command);
            return result;
        }

        public async Task NotifyErrorAsync<T>(T error)
            where T : ErrorNotification
        {
            _logger.LogWarning(_localizer["Error {@error} notified."], error);
            await _mediator.Publish(error);
        }
    }
}
