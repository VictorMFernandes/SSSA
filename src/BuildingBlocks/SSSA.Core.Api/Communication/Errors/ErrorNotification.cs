using MediatR;

namespace SSSA.Core.Api.Communication.Errors
{
    /// <summary>
    /// Generic system error notification.
    /// </summary>
    public class ErrorNotification : INotification
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public ErrorNotification(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}