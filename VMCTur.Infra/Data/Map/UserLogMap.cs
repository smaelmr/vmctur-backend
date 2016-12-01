using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMCTur.Domain.Entities.Users;

namespace VMCTur.Infra.Data.Map
{
    public class UserLogMap : EntityTypeConfiguration<UserLog>
    {
        public UserLogMap()
        {
            ToTable("UserLog");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();
        }
    }
}
