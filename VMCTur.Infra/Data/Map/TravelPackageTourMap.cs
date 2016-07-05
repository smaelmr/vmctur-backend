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

            Property(x => x.GuideTourId)
               .IsRequired();

            Property(x => x.VehicleUsedId)
                .IsRequired();

            Ignore(x => x.TourName);
            Ignore(x => x.DateStart);
            Ignore(x => x.HourStart);

            HasRequired(x => x.VehicleUsed);
            HasRequired(x => x.GuideTour);
        }
    }
}
