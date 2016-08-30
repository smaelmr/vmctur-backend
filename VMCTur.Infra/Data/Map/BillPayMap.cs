using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using VMCTur.Domain.Entities.Financial.BillsPay;

namespace VMCTur.Infra.Data.Map
{
    public class BillPayMap : EntityTypeConfiguration<BillPay>
    {
        public BillPayMap()
        {
            ToTable("BillPay");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            HasRequired(x => x.Reservation);
            Ignore(x => x.Status);
            Ignore(x => x.CustomerName);

        }
    }
}
