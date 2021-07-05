using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace UnivIntel.PostgreSQL.ORM.Core.Handlers
{
    public  class QueryFilterHandler
    {
        public  string ProccessFilter(QueryFilter filter, string query = null, bool child = false)
        {
            if (filter == null) return null;

            var isFirst = true;
            foreach (var item in filter.Filters)
            {
                if (isFirst)
                {
                    isFirst = false;
                    query = CreateExpression(item);
                    continue;
                }

                var logicalOperator = string.IsNullOrWhiteSpace(filter.Logic) ? "AND" : filter.Logic.ToUpper();

                query += item.HasInnerFilters() ? $" {logicalOperator} ({ProccessFilter(item, null, true)})" : CreateExpression(item, logicalOperator);
            }

            if (query != null && !child) return $"WHERE {query}";
            if (query != null && child) return query;

            return null;
        }

        private  string ConvertValueToQueryValue(string @operator, object value)
        {
            //TODO: Add string values checking for SqlInjection.
            switch (@operator)
            {
                //operators without value
                case "IS NULL":
                case "IS NOT NULL":
                    return "";
            }
            return new ObjectConverterHandler().ObjectValueToPostgreString(value);
        }

        private  string CreateExpression(QueryFilter item, string logicalOperator = null)
        {
            var @operator = !string.IsNullOrEmpty(logicalOperator) ? $" {logicalOperator} " : "";

            if (item.Operator == "IN") return $"{@operator}(\"{item.Field}\" {item.Operator} {new ObjectConverterHandler().ObjectValueToPostgreString_IN(item.Values) })";

            return $"{@operator}(\"{item.Field}\" {item.Operator} {ConvertValueToQueryValue(item.Operator, item.Value)})";
        }

        public  bool TryGetQuery(QueryFilter filter, out string query)
        {
            query = ProccessFilter(filter);
            return !string.IsNullOrWhiteSpace(query);
        }

    }

}
