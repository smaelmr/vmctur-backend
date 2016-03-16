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

            container.RegisterType<ICustomerRepository, ClienteRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustomerService, ClienteService>(new HierarchicalLifetimeManager());

            container.RegisterType<IVehicleRepository, VeiculoRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IVehicleService, VeiculoService>(new HierarchicalLifetimeManager());

            container.RegisterType<IGuideRepository, GuiaRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IGuideService, GuiaService>(new HierarchicalLifetimeManager());

            container.RegisterType<ITourRepository, PasseioRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITourService, PasseioService>(new HierarchicalLifetimeManager());

            container.RegisterType<ITravelPackageRepository, TravelPackageRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITravelPackageService, TravelPackageService>(new HierarchicalLifetimeManager());

        }
    
    }
}
