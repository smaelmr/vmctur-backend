using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using VMCTur.Domain.Entities.Reservations;
using System.Data.Entity.Infrastructure.Annotations;

namespace VMCTur.Infra.Data.Map
{
    public class ReservationMap : EntityTypeConfiguration<Reservation>
    {
        public ReservationMap()
        {
            ToTable("Reservation");

            Property(x => x.Id)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                    .IsRequired();

            Property(x => x.ContractNumber)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName, 
                    new IndexAnnotation(
                        new IndexAttribute("IX_ContractNumber") { IsUnique = true }));

            Ignore(x => x.Tours);
        }
    }
}
