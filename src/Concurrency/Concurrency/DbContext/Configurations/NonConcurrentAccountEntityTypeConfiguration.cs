using System;
using System.Collections.Generic;
using System.Text;
using Concurrency.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Concurrency.DbContext.Configurations
{
    internal class NonConcurrentAccountEntityTypeConfiguration : IEntityTypeConfiguration<NonConcurrentAccount>
    {
        public void Configure(EntityTypeBuilder<NonConcurrentAccount> builder)
        {
            builder.ToTable("NonConcurrentAccounts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
            builder.Property(x => x.Balance).HasColumnName("Balance").HasConversion<double>();
        }
    }
}
