﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using VMCTur.Domain.Entities.Users;

namespace VMCTur.Infra.Data.Map
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("User");

            Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();            

            Property(x => x.Name)
                .HasMaxLength(60)
                .IsRequired();

            Property(x => x.Email)
                .HasMaxLength(160)
                //.HasColumnAnnotation(
                    //IndexAnnotation.AnnotationName,
                    //new IndexAnnotation(new IndexAttribute("IX_EMAIL", 1) { IsUnique = true }))
                .IsRequired();

            Property(x => x.Password)
                .HasMaxLength(32)
                .IsFixedLength();
        }
    }
}
