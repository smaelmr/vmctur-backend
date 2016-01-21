using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMCTur.Domain.Entities.Veiculos;

namespace VMCTur.Infra.Data.Map
{
    public class VeiculoMap : EntityTypeConfiguration<Veiculo>
    {
        public VeiculoMap()
        {
            ToTable("veiculo");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Placa)
                .HasMaxLength(7)
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
