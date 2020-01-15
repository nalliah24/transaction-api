using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace transaction_api.Models
{
    public class MockExpense
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Tax { get; set; }
    }
}
