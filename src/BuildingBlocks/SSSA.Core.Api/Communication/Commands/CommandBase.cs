using MediatR;
using System;

namespace SSSA.Core.Api.Communication.Commands
{
    public abstract class CommandBase<T> : IRequest<T>
    {
        protected DateTime Date { get; }

        protected CommandBase()
        {
            Date = DateTime.Now;
        }
    }
}
