using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Concurrency.DbContext.Configurations;
using Concurrency.Models;
using Microsoft.EntityFrameworkCore;

namespace Concurrency.DbContext
{
    public class BankAccountDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<ConcurrentAccountWithRowVersion> ConcurrentAccountsWithRowVersion { get; protected set; }
        public DbSet<ConcurrentAccountWithToken> ConcurrentAccountsWithToken { get; protected set; }
        public DbSet<NonConcurrentAccount> NonConcurrentAccounts { get; protected set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=EFCoreConcurrency;ConnectRetryCount=0");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ConcurrentAccountWithRowVersionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ConcurrentAccountWithTokenEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NonConcurrentAccountEntityTypeConfiguration());

            //Or use this to read all files in the current assembly that implement IEntityTypeConfiguration<T>
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
