using Microsoft.Practices.Unity;
using VMCTur.Bussiness.Services;
using VMCTur.Domain.Contracts.Repositories;
using VMCTur.Domain.Contracts.Services;
using VMCTur.Infra.Data;
using VMCTur.Infra.Repositories;

namespace VMCTur.Startup
{
    public static class DependencyResolver
    {
        public static void Resolve(UnityContainer container)
        {
            container.RegisterType<AppDataContext, AppDataContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            
        }
    
    }
}
