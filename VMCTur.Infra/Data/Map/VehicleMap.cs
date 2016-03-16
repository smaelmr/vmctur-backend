using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMCTur.Domain.Entities.Vehicles;

namespace VMCTur.Infra.Data.Map
{
    public class VehicleMap : EntityTypeConfiguration<Vehicle>
    {
        public VehicleMap()
        {
            ToTable("Vehicle");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Plate)
                .HasMaxLength(8)
                .IsRequired();

            Property(x => x.Model)
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.Year)                
                .IsRequired();

            Property(x => x.NumberOfPassengers)
                .IsRequired();

        }
    }
}
