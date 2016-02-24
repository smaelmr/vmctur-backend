
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using VMCTur.Domain.Entities.Clientes;
using VMCTur.Domain.Entities.Pacotes;

namespace VMCTur.Infra.Data.Map
{
    public class PacoteMap : EntityTypeConfiguration<Pacote>
    {
        public PacoteMap()
        {
            ToTable("pacote");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.ClienteId)                
                .IsRequired();

            Property(x => x.GuiaPasseioId)                        
                .IsRequired();

            Property(x => x.VeiculoUtilizadoId)
                .IsRequired();

            HasMany(x => x.Participantes)
                .WithRequired(x => x.Pacote);
        }
    }
}
