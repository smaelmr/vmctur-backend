﻿
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using VMCTur.Domain.Entities.TravelPackages;

namespace VMCTur.Infra.Data.Map
{
    public class PacoteMap : EntityTypeConfiguration<TravelPackage>
    {
        public PacoteMap()
        {
            ToTable("TravelPackage");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.CustomerId)                
                .IsRequired();

            Property(x => x.GuideTourId)              
                .IsRequired();

            Property(x => x.VehicleUsedId)
                .IsRequired();

            HasMany(x => x.Participants)
                .WithRequired(x => x.TravelPackage);
        }
    }
}
