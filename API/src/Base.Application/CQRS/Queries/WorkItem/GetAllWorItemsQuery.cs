using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Application.CQRS.Queries
{
    /// <summary>
    /// Represents a query to retrieve all work items with optional pagination and filters.
    /// Inherits from <see cref="PaginationQueryFilter"/> to support pagination functionality.
    /// </summary>
    public class GetAllWorkItemsQuery : PaginationQueryFilter
    {
        /// <summary>
        /// Gets or sets the title of the work item for filtering results.
        /// This is an optional filter to search work items based on their title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the start date for filtering work items based on their due date.
        /// This is an optional filter to include work items with a due date on or after this date.
        /// </summary>
        public DateTime? CreationDateStart { get; set; }

        /// <summary>
        /// Gets or sets the end date for filtering work items based on their due date.
        /// This is an optional filter to include work items with a due date on or before this date.
        /// </summary>
        public DateTime? CreationDateEnd { get; set; }

        /// <summary>
        /// Gets or sets the start date for filtering work items based on their due date.
        /// This is an optional filter to include work items with a due date on or after this date.
        /// </summary>
        public DateTime? DueDateStart { get; set; }

        /// <summary>
        /// Gets or sets the end date for filtering work items based on their due date.
        /// This is an optional filter to include work items with a due date on or before this date.
        /// </summary>
        public DateTime? DueDateEnd { get; set; }

        /// <summary>
        /// Gets or sets the user ID for filtering work items assigned to a specific user.
        /// This is an optional filter to retrieve work items assigned to a particular user.
        /// </summary>
        public Guid? IdUser { get; set; }

        /// <summary>
        /// Gets or sets the status ID for filtering work items by their status.
        /// This is an optional filter to retrieve work items with a specific status.
        /// </summary>
        public Guid? IdStatus { get; set; }
    }
}
