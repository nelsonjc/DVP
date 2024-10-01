namespace TaskingSystem.Application.CQRS.Queries
{
    public class GetStatusAllQuery
    {
        public bool Active { get; set; }
        public string Entity { get; set; }
        public bool All { get; set; }
    }
}
