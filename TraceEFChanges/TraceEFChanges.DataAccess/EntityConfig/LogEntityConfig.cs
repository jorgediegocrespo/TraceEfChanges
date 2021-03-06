﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TraceEFChanges.DataAccess.Model;

namespace TraceEFChanges.DataAccess.EntityConfig
{
    public class LogEntityConfig
    {
        public static void SetEntityBuilder(EntityTypeBuilder<LogEntity> entityBuilder)
        {
            //The database table name is 'Admins'
            entityBuilder.ToTable("Logs");

            //Id property is the primary key and it´s required
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).IsRequired();
        }
    }
}
