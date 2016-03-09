using System.Data.Entity;
using VMCTur.Domain.Entities.Clientes;
using VMCTur.Domain.Entities.Guias;
using VMCTur.Domain.Entities.Pacotes;
using VMCTur.Domain.Entities.Passeios;
using VMCTur.Domain.Entities.Users;
using VMCTur.Domain.Entities.Veiculos;
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
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Guia> Guias { get; set; }
        public DbSet<Passeio> Passeios { get; set; }
        public DbSet<Pacote> Pacotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());

            modelBuilder.Configurations.Add(new ClienteMap());
            //modelBuilder.Entity<Cliente>().Ignore(d => d.Idade);

            modelBuilder.Configurations.Add(new VeiculoMap());

            modelBuilder.Configurations.Add(new GuiaMap());            

            modelBuilder.Configurations.Add(new PasseioMap());

            modelBuilder.Configurations.Add(new PacoteMap());

            
        }
    }
}
