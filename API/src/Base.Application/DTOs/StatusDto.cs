using System.ComponentModel.DataAnnotations.Schema;

namespace TaskingSystem.Application.DTOs
{
    public class StatusDto
    {
        public Guid Id { get; set; }
        public string Entity { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }
    }
}
