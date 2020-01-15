using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transaction_api.Models
{
    public class TransactionDto
    {
        public User User { get; set; }
        public Transaction[] Transactions { get; set; }
    }
}
