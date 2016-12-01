using System;
using System.Collections.Generic;
using VMCTur.Domain.Entities.Users;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IUserRepository : IDisposable
    {
        User Get(string email);
        User Get(int id);
        List<User> Get(int skip, int take);
        void Create(User user);
        void Update(User user);
        void Delete(User user);

        void LogRegister(DateTime date, string actionDescription, string user);
    }
}
