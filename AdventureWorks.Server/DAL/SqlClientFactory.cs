using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace AdventureWorks.Server.DAL
{
    public class SqlClientFactory : ISqlClientFactory
    {
        private readonly string _connectionString;

        public SqlClientFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlClientFactory(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection") 
                ?? throw new ArgumentNullException(nameof(_connectionString), "Connection string cannot be null or empty.");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}