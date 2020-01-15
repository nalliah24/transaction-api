using System.Data;

namespace transaction_api.Utils
{
    public interface ISqlConnHelper
    {
        IDbConnection Connection { get; }
    }
}
