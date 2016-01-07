using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Models.Users;

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
    }
}
