﻿using System.Linq.Expressions;
using TaskingSystem.Domain.Entities;

namespace TaskingSystem.Domain.Interfaces.Repository
{
    /// <summary>
    /// Repository interface for performing operations related to the <see cref="User"/> entity.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        public Task<IEnumerable<User>> GetByRol(string rolName);
        public Task ChangePassword (Guid  idUser, string password, Guid idUserUpdated);

        public  Task ActivateAsync(Guid id, Guid idUserUpdated);
    }
}
