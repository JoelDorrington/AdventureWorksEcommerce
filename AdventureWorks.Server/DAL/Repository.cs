using AdventureWorks.Server.Entities;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using AdventureWorks.Server.DAL.QueryParameters;

namespace AdventureWorks.Server.DAL
{
    public class Repository<T>(ISqlClientFactory sqlClientFactory) : IGenericRepository<T> where T : notnull, BaseEntity, IFromId<T>
    {
        private readonly ISqlClientFactory _sqlClientFactory = sqlClientFactory;
        private readonly TableAttribute _tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>() 
            ?? throw new ArgumentNullException($"TableAttribute of type argument T cannot be null. T = {typeof(T).Name}");
        private readonly string _primaryKey = typeof(T).GetProperties()
            .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null)?.Name
            ?? throw new ArgumentNullException($"Primary key of type argument T cannot be null. T = {typeof(T).Name}");

        private readonly Dictionary<string, PropertyInfo> _properties = typeof(T).GetProperties()
            .ToDictionary(t => t.Name, t => t);
        public Task<IReadOnlyList<T>> GetAllAsync()
        {
            return GetAllAsync(new GetParameters());
        }
        public async Task<IReadOnlyList<T>> GetAllAsync(GetParameters args)
        {
            using (var connection = _sqlClientFactory.GetConnection())
            {
                var queryBuilder = new SqlSelectBuilder(_tableAttribute);
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    // SELECT * FROM Production.ProductCategory as c JOIN Production.ProductSubcategory as s ON c.ProductCategoryId = s.ProductCategoryId
                    // SELECT * FROM Production.Product as p WHERE p.ProductSubcategoryId = 1
                    command.CommandText = $"SELECT ";

                    queryBuilder.AddSelect(args.Select);
                    queryBuilder.AddJoin(args.JoinParams);
                    if(args.Filter != null) queryBuilder.AddFilter(args.Filter);
                    queryBuilder.AddOrderBy(args.OrderBy, args.Ascending);
                    queryBuilder.AddPaging(args.Page, args.PageSize);


                    command.CommandText = queryBuilder.BuildQuery();

                    using (var reader = await command.ExecuteReaderAsync())
                    {   
                        List<T> result = new List<T>();
                        while (await reader.ReadAsync())
                        {
                            result.Add(UnpackReader(reader));
                        }
                        return result;
                    }
                }
            }

        }

        private T UnpackReader(SqlDataReader reader)
        {
            int pko = reader.GetOrdinal(_primaryKey);
            var entity = T.FromId(reader.GetInt32(pko));
            for(int i = 0; i < reader.FieldCount; i++)
            {
                string fieldName = reader.GetName(i);
                var property = _properties[fieldName];
                if (!property.CanWrite) continue;
                if (fieldName == _primaryKey) continue;
                property.SetValue(entity, reader[i]);

            }
            return entity;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }
        public async Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
