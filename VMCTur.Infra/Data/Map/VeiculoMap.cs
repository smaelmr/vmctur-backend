using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Entities.Vehicles;

namespace VMCTur.Infra.Data.Map
{
    public class VeiculoMap : EntityTypeConfiguration<Vehicle>
    {
        public VeiculoMap()
        {
            ToTable("Vehicle");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Placa)
                .HasMaxLength(8)
                .IsRequired();

            Property(x => x.Modelo)
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.Ano)                
                .IsRequired();

            Property(x => x.CapacidadePassageiros)
                .IsRequired();

        }
    }
}
