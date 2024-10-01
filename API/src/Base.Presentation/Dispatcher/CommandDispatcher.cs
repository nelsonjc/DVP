using TaskingSystem.Application.CQRS.Common;

namespace TaskingSystem.Presentation.Dispatcher
{
    ///<inheritdoc/>
    public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation)
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TCommandResult>>();
            return handler.Handle(command, cancellation);
        }
    }
}
