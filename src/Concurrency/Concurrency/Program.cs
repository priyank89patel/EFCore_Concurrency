using System;
using System.Threading;
using System.Threading.Tasks;
using Concurrency.DbContext;
using Concurrency.Models;
using Concurrency.Utils;
using Microsoft.EntityFrameworkCore;

namespace Concurrency
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var dbContext = new BankAccountDbContext())
            {
                dbContext.Database.Migrate();
                await dbContext.NonConcurrentAccounts.AddAsync(new NonConcurrentAccount { Balance = 1000.0m });
                await dbContext.ConcurrentAccountsWithToken.AddAsync(new ConcurrentAccountWithToken { Balance = 1000.0m });
                await dbContext.ConcurrentAccountsWithRowVersion.AddAsync(new ConcurrentAccountWithRowVersion { Balance = 1000.0m });
                await dbContext.SaveChangesAsync();
            };

            Console.WriteLine("========== Concurrency Test with NonConcurrent Account ==============================");
            await TestWithoutConcurrencyControl();

            Console.WriteLine("\n\n========== Concurrency Test with Concurrent Account using Concurrent Token ==========");
            await ConcurrencyControlByConcurrencyToken();

            Console.WriteLine("\n\n========== Concurrency Test with Concurrent Account using Row Version ===============");
            await ConcurrencyControlByRowVersion();
        }

        private static async Task TestWithoutConcurrencyControl()
        {
            using (var dbContext = new BankAccountDbContext())
            {
                var account = await dbContext.NonConcurrentAccounts.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (Before):{account.Balance}");
            };

            var threads = new Thread[2];

            threads[0] = new Thread(async () =>
              {
                  using (var dbContext = new BankAccountDbContext())
                  {
                      var account = await dbContext.NonConcurrentAccounts.FindAsync(1);
                      account.Credit(100);
                      await dbContext.SaveChangesAsync();
                  };
              });

            threads[1] = new Thread(async () =>
            {
                using (var dbContext = new BankAccountDbContext())
                {
                    var account = await dbContext.NonConcurrentAccounts.FindAsync(1);
                    account.Debit(200);
                    await dbContext.SaveChangesAsync();
                };
            });

            foreach (var t in threads)
            {
                t.Start();
            }

            Thread.Sleep(1000);
            using (var dbContext = new BankAccountDbContext())
            {
                var account = await dbContext.NonConcurrentAccounts.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (After):{account.Balance}");
            };
        }

        private static async Task ConcurrencyControlByConcurrencyToken()
        {
            using (var dbContext = new BankAccountDbContext())
            {
                var account = await dbContext.ConcurrentAccountsWithToken.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (Before):{account.Balance}");
            };

            var threads = new Thread[2];

            threads[0] = new Thread(async () =>
            {
                using (var dbContext = new BankAccountDbContext())
                {
                    var account = await dbContext.ConcurrentAccountsWithToken.FindAsync(1);
                    account.Credit(100);

                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        ConsoleUtils.WriteErr(ex.Message);
                    }
                };
            });

            threads[1] = new Thread(async () =>
            {
                using (var dbContext = new BankAccountDbContext())
                {
                    var account = await dbContext.ConcurrentAccountsWithToken.FindAsync(1);
                    account.Debit(200);

                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        ConsoleUtils.WriteErr(ex.Message);
                    }
                };
            });

            foreach (var t in threads)
            {
                t.Start();
            }

            Thread.Sleep(1000);

            using (var dbContext = new BankAccountDbContext())
            {
                var account = await dbContext.ConcurrentAccountsWithToken.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (After):{account.Balance}");
            };
        }

        private static async Task ConcurrencyControlByRowVersion()
        {
            using (var dbContext = new BankAccountDbContext())
            {
                var account = await dbContext.ConcurrentAccountsWithRowVersion.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (Before):{account.Balance}");
            };

            var threads = new Thread[2];

            threads[0] = new Thread(async () =>
              {
                  using (var dbContext = new BankAccountDbContext())
                  {
                      var account = await dbContext.ConcurrentAccountsWithRowVersion.FindAsync(1);
                      account.Credit(100);

                      try
                      {
                          await dbContext.SaveChangesAsync();
                      }
                      catch (DbUpdateConcurrencyException ex)
                      {
                          ConsoleUtils.WriteErr(ex.Message);
                      }
                  };
              });

            threads[1] = new Thread(async () =>
            {
                using (var dbContext = new BankAccountDbContext())
                {
                    var account = await dbContext.ConcurrentAccountsWithRowVersion.FindAsync(1);
                    account.Debit(200);

                    try
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        ConsoleUtils.WriteErr(ex.Message);
                    }
                };
            });

            foreach (var t in threads)
            {
                t.Start();
            }

            Thread.Sleep(1000);

            using (var dbContext = new BankAccountDbContext())
            {
                var account = await dbContext.ConcurrentAccountsWithRowVersion.FindAsync(1);
                ConsoleUtils.WriteInf($"Account Balance (After):{account.Balance}");
            };
        }
    }
}
