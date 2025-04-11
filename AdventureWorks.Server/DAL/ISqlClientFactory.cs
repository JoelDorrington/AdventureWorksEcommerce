using Microsoft.Data.SqlClient;

namespace AdventureWorks.Server.DAL
{
    public interface ISqlClientFactory
    {
         SqlConnection GetConnection();
    }
}
