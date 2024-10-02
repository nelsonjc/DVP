namespace TaskingSystem.Application.CQRS.Queries
{
    public class GetUserFilterQuery
    {
        public string? FullName { get; set; }
        public bool? AllRows { get; set; }
    }
}
