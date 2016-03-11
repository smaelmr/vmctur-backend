﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMCTur.Domain.Entities.Tours;

namespace VMCTur.Infra.Data.Map
{
    public class PasseioMap : EntityTypeConfiguration<Tour>
    {
        public PasseioMap()
        {
            ToTable("Tour");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Nome)
                .HasMaxLength(100)
                .IsRequired();

            Property(x => x.HorarioAbertura)
                .IsRequired();

            Property(x => x.HorarioFechamento)                
                .IsRequired();

        }
    }
}
