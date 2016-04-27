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

            container.RegisterType<ICustomerRepository, CustomerRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustomerService, CustomerService>(new HierarchicalLifetimeManager());

            container.RegisterType<IVehicleRepository, VehicleRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IVehicleService, VehicleService>(new HierarchicalLifetimeManager());

            container.RegisterType<IGuideRepository, TourGuideRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IGuideService, TourGuideService>(new HierarchicalLifetimeManager());

            container.RegisterType<ITourRepository, TourRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITourService, TourService>(new HierarchicalLifetimeManager());            

            container.RegisterType<ITravelPackageRepository, TravelPackageRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITravelPackageService, TravelPackageService>(new HierarchicalLifetimeManager());

            container.RegisterType<IBillReceiveRepository, BillReceiveRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBillReceiveService, BillReceiveService>(new HierarchicalLifetimeManager());

            container.RegisterType<ITourScheduleRepository, TourScheduleRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITourScheduleService, TourScheduleService>(new HierarchicalLifetimeManager());

            container.RegisterType<ITravelPackageTourRepository, TravelPackageTourRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ITravelPackageParticipantRepository, TravelPackageParticipantRepository>(new HierarchicalLifetimeManager());

        }
    
    }
}
