using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMCTur.Domain.Entities.TourGuides;

namespace VMCTur.Infra.Data.Map
{
    public class TourGuideMap : EntityTypeConfiguration<TourGuide>
    {
        public TourGuideMap()
        {
            ToTable("TourGuide");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Name)
                .HasMaxLength(60)
                .IsRequired();

            Ignore(x => x.BondTypeDisplay);
                

        }
    }
}
