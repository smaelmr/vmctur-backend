using System.Data.Entity;
using VMCTur.Domain.Entities.Customers;
using VMCTur.Domain.Entities.TourGuides;
using VMCTur.Domain.Entities.TravelPackages;
using VMCTur.Domain.Entities.Tours;
using VMCTur.Domain.Entities.Users;
using VMCTur.Domain.Entities.Vehicles;
using VMCTur.Infra.Data.Map;

namespace VMCTur.Infra.Data
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class AppDataContext : DbContext
    {
        public AppDataContext() : base("AppConnectionString")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;        
                        
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<TourGuide> TourGuides { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<TravelPackage> TravelPackages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new ClienteMap());
            modelBuilder.Configurations.Add(new VeiculoMap());
            modelBuilder.Configurations.Add(new GuiaMap());            
            modelBuilder.Configurations.Add(new PasseioMap());
            modelBuilder.Configurations.Add(new TravelPackageMap());            
        }
    }
}
