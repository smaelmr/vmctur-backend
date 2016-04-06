using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using VMCTur.Domain.Entities.Financial.BillsReceive;

namespace VMCTur.Infra.Data.Map
{
    public class BillReceiveMap : EntityTypeConfiguration<BillReceive>
    {
        public BillReceiveMap()
        {
            ToTable("BillReceive");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Ignore(x => x.Status);

        }
    }
}
