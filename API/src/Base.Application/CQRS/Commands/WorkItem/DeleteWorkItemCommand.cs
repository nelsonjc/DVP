﻿namespace TaskingSystem.Application.CQRS.Commands
{
    public class DeleteWorkItemCommand
    {
        public Guid IdTask { get; set; }
        public Guid IdUserUpdated { get; set; }
    }
}
