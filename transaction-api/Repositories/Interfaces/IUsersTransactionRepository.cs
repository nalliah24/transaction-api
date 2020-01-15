using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using transaction_api.Models;
using transaction_api.Utils;

namespace transaction_api.Repositories
{
    public interface IUsersTransactionRepository
    {
        Task<Result<IEnumerable<Transaction>>> GetTransactionsByUserId(string userId);
    }
}
