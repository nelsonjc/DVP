namespace TaskingSystem.Application.CQRS.Common
{
    /// <summary>
    /// This interface is responsible for handling query operations
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TQueryResult"></typeparam>
    public interface IQueryHandler<in TQuery, TQueryResult>
    {
        /// <summary>
        /// single Handle method that takes a query object of type TQuery and a cancellation token
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellation"></param>
        /// <returns>Task representing the result of the query operation.</returns>
        Task<TQueryResult> Handle(TQuery query, CancellationToken cancellation);
    }
}
