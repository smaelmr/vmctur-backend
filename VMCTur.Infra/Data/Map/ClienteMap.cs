using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using VMCTur.Domain.Entities.Customers;

namespace VMCTur.Infra.Data.Map
{
    public class ClienteMap : EntityTypeConfiguration<Customer>
    {
        public ClienteMap()
        {
            ToTable("Customer");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Nome)
                .HasMaxLength(60)
                .IsRequired();

            Property(x => x.Email)
                .HasMaxLength(160)               
                .IsRequired();

            Ignore(x => x.Idade);

        }
    }
}
