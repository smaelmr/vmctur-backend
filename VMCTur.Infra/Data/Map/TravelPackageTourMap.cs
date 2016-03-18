using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Infra.Data.Map
{
    public class TravelPackageTourMap : EntityTypeConfiguration<TravelPackageTour>
    {
        public TravelPackageTourMap()
        {
            
            ToTable("TravelPackageTour");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Ignore(x => x.TourName);
        }
    }
}
