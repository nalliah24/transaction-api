using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using transaction_api.Models;
using transaction_api.Utils;
using Dapper;

namespace transaction_api.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ISqlConnHelper _sqlConnHelper;

        public TransactionRepository(ISqlConnHelper sqlConnHelper)
        {
            _sqlConnHelper = sqlConnHelper;
        }

        public async Task<Result> Create(Transaction[] transactionsDto)
        {
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    string sqlTransInsert = @"insert into transactions (user_id, trans_type, description, amount, tax, trans_date, category, status)
                                values (@userid, @transtype, @description, @amount, @tax, @transdate, @category, @status);";

                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        foreach (Transaction tran in transactionsDto)
                        {
                            DynamicParameters dp = new DynamicParameters();
                            Constants.TransactionStatus statusNew = Constants.TransactionStatus.New;
                            string insertStatus = statusNew.ToString("g");
                            DateTime updatedDateTime = DateTime.Now;

                            dp.Add("userid", tran.UserId, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                            dp.Add("transtype", tran.TransType, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                            dp.Add("description", tran.Description, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                            dp.Add("amount", tran.Amount, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
                            dp.Add("tax", tran.Tax, System.Data.DbType.Decimal, System.Data.ParameterDirection.Input);
                            dp.Add("transdate", tran.TransDate, System.Data.DbType.Date, System.Data.ParameterDirection.Input);
                            dp.Add("category", tran.Category, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                            dp.Add("status", insertStatus, System.Data.DbType.String, System.Data.ParameterDirection.Input);

                            var result = await conn.ExecuteAsync(sqlTransInsert, dp, transaction, commandType: System.Data.CommandType.Text);
                        }

                        transaction.Commit();
                        return new Result() { IsSuccess = true };

                    } catch(Exception ex)
                    {
                        transaction.Rollback();
                        return new Result() { IsSuccess = false, Error = ex.Message };
                    }
                }

            } catch(Exception ex)
            {
                return new Result() { IsSuccess = false, Error = ex.Message };
            }
        }

        public async Task<Result> Update(TransactionUpdateStatus[] transactionUpdateStatuses)
        {
            try
            {
                using (var conn = _sqlConnHelper.Connection)
                {
                    string sqlTransUpdate = @"update transactions 
                                                    set status = @status,
                                                        updated_date = GETDATE()
                                                where id = @id";

                    conn.Open();
                    var transaction = conn.BeginTransaction();
                    try
                    {
                        foreach (TransactionUpdateStatus item in transactionUpdateStatuses)
                        {
                            DynamicParameters dp = new DynamicParameters();
                            dp.Add("status", item.Status, System.Data.DbType.String, System.Data.ParameterDirection.Input);
                            dp.Add("id", item.Id, System.Data.DbType.Guid, System.Data.ParameterDirection.Input);
                            await conn.ExecuteAsync(sqlTransUpdate, dp, transaction, commandType: System.Data.CommandType.Text);
                        }
                        transaction.Commit();
                        return new Result() { IsSuccess = true };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new Result() { IsSuccess = false, Error = ex.Message };
                    }
                }

            }
            catch (Exception ex)
            {
                return new Result() { IsSuccess = false, Error = ex.Message };
            }
        }
    }
}
