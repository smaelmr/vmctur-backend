using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Infra.Data.Map
{
    public class TravelPackageParticipantMap : EntityTypeConfiguration<TravelPackageParticipant>
    {
        public TravelPackageParticipantMap()
        {
            
            ToTable("TravelPackageParticipant");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();            
        }
    }
}
