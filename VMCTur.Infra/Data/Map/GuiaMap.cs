using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Entities.TourGuides;
using VMCTur.Domain.Entities.Enums;

namespace VMCTur.Infra.Data.Map
{
    public class GuiaMap : EntityTypeConfiguration<TourGuide>
    {
        public GuiaMap()
        {
            ToTable("TourGuide");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Nome)
                .HasMaxLength(60)
                .IsRequired();

            Ignore(x => x.TipoVinculoDisplay);
                

        }
    }
}
