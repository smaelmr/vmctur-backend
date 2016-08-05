using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Infra.Data.Map
{
    public class TravelPackageMap : EntityTypeConfiguration<TravelPackage>
    {
        public TravelPackageMap()
        {

            ToTable("TravelPackage");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.CustomerId)
                .IsRequired();
            
            HasMany(x => x.Participants)
                .WithRequired(x => x.TravelPackage);

            HasMany(x => x.Tours)
                .WithRequired(x => x.TravelPackage);

            HasMany(x => x.Bills)
                 .WithRequired(x => x.TravelPackage);
            
            HasRequired(x => x.Customer);            

        }
    }
}
