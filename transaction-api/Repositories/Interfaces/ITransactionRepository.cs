using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using transaction_api.Models;
using transaction_api.Utils;

namespace transaction_api.Repositories
{
    public interface ITransactionRepository
    {
        Task<Result> Create(Transaction[] transactionsDto);
        Task<Result> Update(TransactionUpdateStatus[] transactionUpdateStatuses);
    }
}
