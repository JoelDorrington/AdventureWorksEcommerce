using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace AdventureWorks.Server.DAL
{
    public class ProductsRepository
    {
        private readonly string _connectionString;

        public ProductsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Product> GetPaged(int startRowIndex, int maximumRows, string sortExpression, out int totalRowCount)
        {
            var products = new List<Product>();
            totalRowCount = 0;

            string sortColumn = "Name";
            string sortDirection = "ASC";

            if (!string.IsNullOrEmpty(sortExpression))
            {
                var parts = sortExpression.Split(' ');
                if (parts.Length == 2)
                {
                    sortColumn = parts[0];
                    sortDirection = parts[1].ToUpper() == "DESC" ? "DESC" : "ASC";
                }
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("GetPagedProducts", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@StartRowIndex", startRowIndex);
                    command.Parameters.AddWithValue("@MaximumRows", maximumRows);
                    command.Parameters.AddWithValue("@SortColumn", sortColumn);
                    command.Parameters.AddWithValue("@SortDirection", sortDirection);

                    var totalRowsParam = new SqlParameter("@TotalRowCount", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(totalRowsParam);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                                ThumbnailPhotoUrl = reader.GetString(reader.GetOrdinal("ThumbnailPhotoUrl"))
                            });
                        }
                    }

                    totalRowCount = (int)totalRowsParam.Value;
                }
            }

            return products;
        }
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = "";
        public decimal ListPrice { get; set; }
        public string ThumbnailPhotoUrl { get; set; } = "";
    }
}
