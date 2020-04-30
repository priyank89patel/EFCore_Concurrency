using System;
using System.Collections.Generic;
using System.Text;
using Concurrency.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concurrency.DbContext.Configurations
{
    internal class ConcurrentAccountWithRowVersionEntityTypeConfiguration : IEntityTypeConfiguration<ConcurrentAccountWithRowVersion>
    {
        public void Configure(EntityTypeBuilder<ConcurrentAccountWithRowVersion> builder)
        {
            builder.ToTable("ConcurrentAccountsWithRowVersion");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Balance).HasColumnName("Balance").HasColumnType("money");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").IsRowVersion();

        }
    }
}
