using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using transaction_api.Models;
using transaction_api.Repositories;
using transaction_api.Utils;

namespace transaction_api.Controllers
{
    // [Route("api/[controller]")]
    [Route("api/users/{userid}/transactions")]
    [ApiController]
    public class UserTransactionsController : ControllerBase
    {
        private readonly IUsersTransactionRepository _usersTransactionRepository;

        public UserTransactionsController(IUsersTransactionRepository usersTransactionRepository)
        {
            _usersTransactionRepository = usersTransactionRepository;
        }

        // GET api/users/{user1}/transactions
        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<Transaction>>>> GetTransactionsByUserId(string userId)
        {
            Result<IEnumerable<Transaction>> resultTransactions = await _usersTransactionRepository.GetTransactionsByUserId(userId);
            if (resultTransactions.Entity == null || resultTransactions.Entity.Count() == 0)
            {
                resultTransactions.IsSuccess = false;
                resultTransactions.AddError("Transaction not found for the provided id.");
                return NotFound(resultTransactions);
            }
            resultTransactions.IsSuccess = true;
            return resultTransactions;
        }

    }
}
