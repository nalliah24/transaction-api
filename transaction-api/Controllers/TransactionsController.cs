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
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Result>> Post([FromBody] TransactionDto transactionDto)
        {
            Result<bool> result = ValidateUser(transactionDto.User);
            if (!result.Entity)
            {
                return BadRequest(result);
            }

            try
            {
                // Ensure user already exists? If not add the user to the user table...
                // TODO: For production, validate connectiong to using UserProfile API. Use a flag to use in PROD and NOT in DEV mode
/*                Result<bool> userFoundResult = await _userRepository.IsUserExists(transactionDto.User);
                if (!userFoundResult.Entity)
                {
                    // Create new user
                    Result userResult = await _userRepository.Create(transactionDto.User);
                    if (!userResult.IsSuccess)
                    {
                        // if user creation is unsuccessfull, no need to proceed and save transactions..
                        return BadRequest(userResult);
                    }
                }*/

                // Create transactions..
                Result tranResult = await _transactionRepository.Create(transactionDto.Transactions);
                if (tranResult.IsSuccess == true)
                {
                    return Created("", "Transaction(s) have been saved successfully");
                }
                else
                {
                    return BadRequest(tranResult);
                }
            }
            catch (Exception ex)
            {
                Result resultErr = new Result() { Error = $"Error creating transactions. {ex.Message}" };
                return resultErr;
            }
        }

        // PUT: api/Transactions/[arrayoftrans]
        [HttpPut]
        public async Task<ActionResult<Result>> Put([FromBody] TransactionUpdateStatus[] transactionUpdateStatuses)
        {
            if (transactionUpdateStatuses == null || transactionUpdateStatuses.Length <= 0)
            {
                return BadRequest(new Result() { IsSuccess = false, Error = "Must contain transactions items" }); 
            }

            Result tranUpdateResult = await _transactionRepository.Update(transactionUpdateStatuses);
            if (tranUpdateResult.IsSuccess == true)
            {
                return Ok("Transaction(s) have been updated successfully");
            }
            else
            {
                return BadRequest(tranUpdateResult);
            }
        }

        private Result<bool> ValidateUser(User user)
        {
            Result<bool> result = new Result<bool>();
            result.Entity = true;
            if (user == null || (user.UserId == null || user.UserId == ""))
            {
                result.AddError("User id is required");
            }
            if (result.Errors.Count > 0)
            {
                result.Entity = false;
            }
            return result;
        }

    }
}
