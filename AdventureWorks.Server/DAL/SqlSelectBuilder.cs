using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Server.DAL.QueryParameters;
using System.Data.Common;

namespace AdventureWorks.Server.DAL
{
    public class SqlSelectBuilder(TableAttribute _tableAttribute)
    {
        private string select = "";
        private string join = "";
        private string where = "";
        private string orderBy = "";
        private string offset = "";
        private string fetch = "";

        public void AddSelect(IEnumerable<SelectParameter> s)
        {
            if (s.Any())
            {
                select = s.Select(s => $"t.{s.Column}")
                    .Aggregate((string a, string b) => $"{a}, {b}");
            }
        }

        public void AddJoin(IEnumerable<IJoinParameter> js)
        {
            if (js.Any())
            {
                    int jIndex = 1;
                    join = js.Select(j => $"{j.JoinType.ToString().ToUpper()} JOIN [{j.Schema}].[{j.Table}] AS [j{jIndex++}] ON t.{j.LocalKey} = j{jIndex - 1}.{j.ForeignKey}")
                        .Aggregate((string a, string b) => $"{a}, {b}");

                    jIndex = 1;
                    foreach (IJoinParameter j in js)
                    {
                        if (j.SelectParameters.Length == 0) continue;
                        if (select.Length > 0) select += ", ";
                        select += j.SelectParameters.Select(s => $"j{jIndex}.{s.Column}{(s.Alias != null ? $" AS {s.Alias}" : "")}")
                            .Aggregate((string a, string b) => $"{a}, {b}");
                    }
            }
        }

        public void AddFilter(ILogicalCondition x)
        {
            string expression = x.ToString();
            if (!string.IsNullOrEmpty(expression))
            {
                where = $"WHERE {expression}";
            }
        }

        public void AddOrderBy(string o)
        {
            if (!string.IsNullOrEmpty(o))
            {
                orderBy = $"ORDER BY {o}";
            }
        }

        public void AddOrderBy(string o, bool ascending)
        {
            if (!string.IsNullOrEmpty(o))
            {
                orderBy = $"ORDER BY {o} {(ascending ? "ASC" : "DESC")}";
            }
        }

        public void AddPaging(int page, int pageSize)
        {
            if (page >= 0 && pageSize > 0)
            {
                offset = $"OFFSET {page * pageSize} ROWS";
                fetch = $"FETCH NEXT {pageSize} ROWS ONLY";
            }
        }

        public string BuildQuery()
        {
            if(string.IsNullOrEmpty(select))
            {
                select = "*";
            }
            string query = $"SELECT {select} FROM [{_tableAttribute.Schema}].[{_tableAttribute.Name}] AS t";
            if (!string.IsNullOrEmpty(join)) query += $" {join}";
            if (!string.IsNullOrEmpty(where)) query += $" {where}";
            if (!string.IsNullOrEmpty(orderBy)) query += $" {orderBy}";
            if (!string.IsNullOrEmpty(offset)) query += $" {offset}";
            if (!string.IsNullOrEmpty(fetch)) query += $" {fetch}";
            return query;
        }
    }
}
