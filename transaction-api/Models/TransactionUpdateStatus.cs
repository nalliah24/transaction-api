using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transaction_api.Models
{
    public class TransactionUpdateStatus
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
