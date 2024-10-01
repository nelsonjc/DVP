namespace TaskingSystem.Application.CQRS.Common
{
    /// <summary>
    ///  This interface is responsible for dispatching queries to their respective query handlers
    /// </summary>
    public interface IQueryDispatcher
    {
        /// <summary>
        /// Generic Dispatch method that takes a query object and a cancellation token
        /// </summary>
        /// <typeparam name="TQuery"></typeparam>
        /// <typeparam name="TQueryResult"></typeparam>
        /// <param name="query"></param>
        /// <param name="cancellation"></param>
        /// <returns>Task representing the result of the dispatched query.</returns>
        Task<TQueryResult> Dispatch<TQuery, TQueryResult>(TQuery query, CancellationToken cancellation);
    }
}
