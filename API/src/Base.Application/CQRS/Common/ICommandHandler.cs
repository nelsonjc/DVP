namespace TaskingSystem.Application.CQRS.Common
{
    /// <summary>
    /// This interface is responsible for handling command operations
    /// </summary>
    public interface ICommandHandler<in TCommand, TCommandResult>
    {
        Task<TCommandResult> Handle(TCommand command, CancellationToken cancellation);
    }
}
