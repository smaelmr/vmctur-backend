using VMCTur.Domain.Entities.Users;

namespace VMCTur.Domain.Contracts.Repositories
{
    public interface IUserLogRepository
    {
        void LogRegistry(UserLog log);
    }
}
