using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transaction_api.Models
{
    public class AutoLoadUserTransactionDto
    {
        public string UserId { get; set; }
        public int NumberOfTransactions { get; set; }
    }
}
