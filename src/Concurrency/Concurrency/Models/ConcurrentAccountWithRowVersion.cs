using System;
using System.Collections.Generic;
using System.Text;

namespace Concurrency.Models
{
    public class ConcurrentAccountWithRowVersion : BankAccount
    {
        public byte[] RowVersion { get; set; }
    }
}
