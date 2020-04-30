using System;
using Concurrency.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Concurrency
{
    class Program
    {
        static void Main(string[] args)
        {
            using (BankAccountDbContext dbContext = new BankAccountDbContext())
            {
                dbContext.Database.Migrate();
            };
        }
    }
}
