namespace TaskingSystem.Application.CQRS.Common
{
    /// <summary>
    /// This interface is responsible for dispatching commands to their respective command handlers.
    /// </summary>
    public interface ICommandDispatcher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <typeparam name="TCommandResult"></typeparam>
        /// <param name="command"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<TCommandResult> Dispatch<TCommand, TCommandResult>(TCommand command, CancellationToken cancellation);
    }
}
