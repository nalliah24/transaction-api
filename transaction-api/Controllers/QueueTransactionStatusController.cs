using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using transaction_api.Models;
using transaction_api.Utils;

namespace transaction_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueTransactionStatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public QueueTransactionStatusController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // POST: api/QueueTransactionStatusUpdate
        [HttpPost]
        public async void Post([FromBody] TransactionUpdateStatus[] transactionUpdateStatuses)
        {
            // Fire and forget
            /** iwr -Method POST -Uri https://ms-expense-react-func-app.azurewebsites.net/api/onExpSubmittedAddToQueToUpdTrans?code=<<...get..from...azure...fn..>> 
            *      -Headers @{ "content-type"="application/json" } 
            *      -Body '
            *         [{"id": "aa2612d0-626c-4fbc-bd2c-052837a9fa0e", "status": "Processed" }, 
            *          {"id": "c5204ebf-23fe-40e3-b582-4887cebdf796", "status": "Processed" }]'
            */
            try
            {
                string url = _configuration.GetSection("Api").GetSection("UpdateTransactionStatus").Value;
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                using (var httpContent = new UtilHttpContent().CreateHttpContent(transactionUpdateStatuses))
                {
                    request.Content = httpContent;
                    using (var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            // var content = await response.Content.ReadAsStringAsync();
                            // return StatusCode((int)response.StatusCode, content);
                            // LOG Success
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // LOG ERROR
                string error = ex.Message.ToString();
            }
        }

    }
}
