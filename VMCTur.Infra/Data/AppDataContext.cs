using SpaUserControl.Infra.Data.Map;
using System.Data.Entity;
using VMCTur.Domain.Models.Users;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
        }
    }
}
