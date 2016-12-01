using System;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Entities.Users;
using VMCTur.Infra.Data;

namespace VMCTur.Infra.Repositories
{
    public class UserLogRepository : IUserLogRepository
    {
        private AppDataContext _context;

        public UserLogRepository(AppDataContext context)
        {
            this._context = context;
        }

        public void LogRegistry(UserLog log)
        {
            _context.UserLog.Add(log);
            _context.SaveChanges();
        }
    }
}
