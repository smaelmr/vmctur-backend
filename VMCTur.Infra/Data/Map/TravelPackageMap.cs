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

            Property(x => x.GuideTourId)              
                .IsRequired();

            Property(x => x.VehicleUsedId)
                .IsRequired();

            HasMany(x => x.Participants)
                .WithRequired(x => x.TravelPackage);

            Ignore(x => x.DateStart);
            Ignore(x => x.HourStart);
        }
    }
}
