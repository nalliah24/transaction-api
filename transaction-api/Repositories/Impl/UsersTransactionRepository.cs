using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using transaction_api.Utils;
using Dapper;
using transaction_api.Models;

namespace transaction_api.Repositories
{
    public class UsersTransactionRepository : IUsersTransactionRepository
    {
        private readonly ISqlConnHelper _sqlConnHelper;

        public UsersTransactionRepository(ISqlConnHelper sqlConnHelper)
        {
            _sqlConnHelper = sqlConnHelper;
        }

        public async Task<Result<IEnumerable<Transaction>>> GetTransactionsByUserId(string userId)
        {
            using (var conn = _sqlConnHelper.Connection)
            {
                Result<IEnumerable<Transaction>> result = new Result<IEnumerable<Transaction>>();
                Result<List<Transaction>> result2 = new Result<List<Transaction>>();
                string sql = @"select t.Id, t.user_id as userid, t.trans_type as transtype, t.description, t.amount, t.tax, t.trans_date as transdate, t.category, lk.description as categoryDescription, t.status 
                                    from transactions t inner join category_lookup lk
                                    on t.category = lk.category
                                    where user_id = @userid
                                    and status='New'";
                DynamicParameters dp = new DynamicParameters();
                dp.Add("userid", userId, System.Data.DbType.String, System.Data.ParameterDirection.Input, 100);

                try
                {
                    conn.Open();
                    var response = await conn.QueryAsync<Transaction>(sql, dp);
                    result.Entity = response;
                    return result;
                }
                catch (Exception ex)
                {
                    // Log error
                    result.AddError(ex.Message);
                    return result;
                    // throw new Exception("Error fetching user transaction data: " + ex.Message);
                }


            }
        }
    }
}
