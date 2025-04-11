using AdventureWorks.Server.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Reflection;

namespace AdventureWorks.Server.DAL.QueryParameters
{
    public enum ComparerOperators
    {
        Equal,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Like,
        In
    }
    public enum LogicalOperator
    {
        AND,
        OR
    }

    public interface ILogicalCondition
    {
        public string ToString();
    }

    public class WhereExpression(ILogicalCondition[]? ps = null, LogicalOperator op = LogicalOperator.AND) : ILogicalCondition
    {
        public ILogicalCondition[] Conditions { get; set; } = ps ?? [];
        public LogicalOperator op { get; set; } = op;
        new public string ToString()
        {
            if (Conditions.Length == 0) return string.Empty;
            return Conditions.Select(c => c.ToString())
                .Aggregate((string a, string b) => $"({a} {op.ToString()} {b})");
        }

        public void AddCondition(ILogicalCondition condition)
        {
            Conditions = Conditions.Concat([condition]).ToArray();
        }

    }
    public class WhereCondition(string Column, ComparerOperators op, string Value) : ILogicalCondition
    {
        public string Column { get; set; } = Column;
        public ComparerOperators Operator { get; set; } = op;
        public string Value { get; set; } = Value;

        public string OperatorToString()
        {
            return Operator switch
            {
                ComparerOperators.Equal => "=",
                ComparerOperators.NotEqual => "<>",
                ComparerOperators.GreaterThan => ">",
                ComparerOperators.GreaterThanOrEqual => ">=",
                ComparerOperators.LessThan => "<",
                ComparerOperators.LessThanOrEqual => "<=",
                ComparerOperators.Like => "LIKE",
                ComparerOperators.In => "IN",
                _ => "=",
            };
        }

        new public string ToString()
        {
            if (string.IsNullOrEmpty(Column)) return string.Empty;
            if (string.IsNullOrEmpty(Value)) return Column;
            return $"t.[{Column}] {OperatorToString()} {Value}";
        }
    }
    public class GetParameters
    {
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 0;
        public string OrderBy { get; set; } = "Id";
        public bool Ascending { get; set; } = true;
        public SelectParameter[] Select { get; set; } = [];
        public IJoinParameter[] JoinParams { get; set; } = [];
        public WhereExpression? Filter { get; set; } = null;
    }
    public class SelectParameter(string? Column = null)
    {
        public string Column { get; set; } = Column ?? string.Empty;
        public string Alias { get; set; } = string.Empty;

        new public string ToString()
        {
            if (string.IsNullOrEmpty(Column)) return string.Empty;
            if (string.IsNullOrEmpty(Alias)) return Column;
            return $"{Column}{(string.IsNullOrEmpty(Alias) ? $" AS {Alias}" : "")}";
        }
    }

    public interface IJoinParameter
    {
        public SupportedJoinOperators JoinType { get; }
        public string Schema { get; }
        public string Table { get; }
        public string LocalKey { get; }
        public string ForeignKey { get; }
        public SelectParameter[] SelectParameters { get; set; }
    }
    public class JoinParameter<T> : IJoinParameter where T : BaseEntity
    {
        public JoinParameter(SupportedJoinOperators? type = null, string? localKey = null)
        {
            var t = typeof(T);
            var tableAttribute = t.GetCustomAttribute<TableAttribute>()
                ?? throw new ArgumentNullException($"TableAttribute of type argument T cannot be null. T = {t.Name}");
            var primaryKey = t.GetProperties()
                .FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null)?.Name
                ?? throw new ArgumentNullException($"Primary key of type argument T cannot be null. T = {t.Name}");

            JoinType = type ?? SupportedJoinOperators.Inner;
            Schema = tableAttribute.Schema ?? "Production";
            Table = tableAttribute.Name;
            ForeignKey = primaryKey;
            LocalKey = localKey ?? primaryKey;
        }
        public SupportedJoinOperators JoinType { get; }
        public string Schema { get; }
        public string Table { get; }
        public string LocalKey { get; }
        public string ForeignKey { get; }
        public SelectParameter[] SelectParameters { get; set; } = [];

    }
}