﻿namespace TaskingSystem.Application.CQRS.Commands
{
    public class DeleteUserCommand
    {
        public Guid IdUser { get; set; }
        public Guid IdUserUpdated { get; set; }
    }
}
