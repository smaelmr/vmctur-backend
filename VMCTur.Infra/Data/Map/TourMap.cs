using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMCTur.Domain.Entities.Tours;

namespace VMCTur.Infra.Data.Map
{
    public class TourMap : EntityTypeConfiguration<Tour>
    {
        public TourMap()
        {
            ToTable("Tour");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.OpenHour)
                .IsRequired();

            Property(x => x.CloseHour)                
                .IsRequired();

        }
    }
}
