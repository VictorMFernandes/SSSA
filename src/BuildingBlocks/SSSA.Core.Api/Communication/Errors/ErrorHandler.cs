using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SSSA.Core.Api.Communication.Errors
{
    public class ErrorHandler : INotificationHandler<ErrorNotification>
    {
        private List<ErrorNotification> _errors;

        public ErrorHandler()
        {
            _errors = new List<ErrorNotification>();
        }

        public Task Handle(ErrorNotification message, CancellationToken cancellationToken)
        {
            _errors.Add(message);
            return Task.CompletedTask;
        }

        public List<ErrorNotification> GetNotifications() => _errors;

        public bool NotificationExists() => GetNotifications().Any();

        public void Dispose() => _errors = new List<ErrorNotification>();
    }
}
