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
    //[Route("api/[controller]")]
    [ApiController]
    public class TransactionsAutoLoadController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionsAutoLoadController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        // DEVLOPMENT MODE ONLY
        // POST: api/TransactionsAutoLoad  // Note: Remove attribute from top Class def..
        [Route("api/transactions/autoload")]
        [HttpPost]
        public async Task<ActionResult<Result<string>>> Post([FromBody] AutoLoadUserTransactionDto autoLoadUserTransactionDto)
        {
            Result<string> response = new Result<string>();
            Transaction[] transactionDtos = GetSampleData(autoLoadUserTransactionDto.UserId, autoLoadUserTransactionDto.NumberOfTransactions);
            try
            {
                Result tranResult = await _transactionRepository.Create(transactionDtos);
                if (tranResult.IsSuccess == true)
                {
                    response.Entity = "Transaction(s) have been saved successfully";
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    return BadRequest(tranResult);
                }
            }
            catch (Exception ex)
            {
                response.Error = $"Error creating transactions. {ex.Message}";
                return response;
            }
        }

        // Helper
        private Transaction[] GetSampleData(string userId, int numOfTrans)
        {
            List<Transaction> list = new List<Transaction>();
            for(int i = 0; i < numOfTrans; i++)
            {
                MockExpense mock = CreateMockExp();
                Transaction tran = new Transaction() {
                    UserId = userId,
                    TransType = "DR",
                    Description = mock.Description,
                    Amount = mock.Amount,
                    Tax = mock.Tax,
                    TransDate = DateTime.Now.AddDays(-3),
                    Category = mock.Category,
                    Status = Constants.TransactionStatus.New.ToString()
                };
                list.Add(tran);
            }
            return list.ToArray();
        }



        private MockExpense CreateMockExp()
        {
            string[] listAccm = new string[4] { "Westin Horbour Castle", "Holiday Inn", "Mariot Plaza At Niagra", "Sheraton Suite" };
            string[] listFood = new string[4] { "Montana Restaurant", "Kellys Fine Dine", "Starbucks", "Chinese Fine Cusine" };
            string[] listTrvl = new string[4] { "Jet Airways", "Air Canada", "British Airways", "US Airways" };

            Random rnd = new Random();
            int idx = rnd.Next(0, 4);
            decimal rndAmount = (decimal)Math.Round(rnd.NextDouble() * 500, 2);
            decimal tax = rndAmount * 0.10M;

            Random rnd2 = new Random();
            int selIdx = rnd2.Next(1, 20);

            MockExpense mock = new MockExpense();
            if (selIdx < 8)
            {
                mock.Category = Constants.CategoryAccm;
                mock.Description = listAccm[idx];
            }
            else if (selIdx < 16)
            {
                mock.Category = Constants.CategoryFood;
                mock.Description = listFood[idx];
            }
            else
            {
                mock.Category = Constants.CategoryTrvl;
                mock.Description = listTrvl[idx];
            }
            mock.Amount = rndAmount;
            mock.Tax = tax;

            return mock;
        }
    }


}
