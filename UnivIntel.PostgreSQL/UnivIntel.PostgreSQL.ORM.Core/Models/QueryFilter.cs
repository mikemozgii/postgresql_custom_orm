using UnivIntel.PostgreSQL.Core;
using UnivIntel.PostgreSQL.ORM.Core.Enums;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnivIntel.PostgreSQL.ORM.Core.Models
{
    /// <summary>
    /// Class for describing filter tree node.
    /// </summary>
    [DebuggerStepThrough]
    public class QueryFilter
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public object Values { get; set; }
        public string Logic { get; set; }
        public List<QueryFilter> Filters { get; set; }

        [DebuggerStepThrough]
        public QueryFilter()
        {
        }

        public QueryFilter(string logic, List<QueryFilter> filters)
        {
            Logic = logic;
            Filters = filters;
        }

        public QueryFilter(string logic, QueryFilter filter)
        {
            Logic = logic;
            Filters =  new List<QueryFilter> { filter };
        }

        public QueryFilter(QueryFilter filter)
        {
            Logic = QueryLogicalOperators.AND;
            Filters = new List<QueryFilter> { filter };
        }

        public QueryFilter(string logic, params QueryFilter[] filters)
        {
            Logic = logic;
            Filters = filters.ToList();
        }
        public QueryFilter(params QueryFilter[] filters)
        {
            Logic = QueryLogicalOperators.AND;
            Filters = filters.ToList();
        }


        [DebuggerStepThrough]
        public static QueryFilter Equal(string field, object value) => new QueryFilter(field, QueryOperators.EQ, value);

        [DebuggerStepThrough]
        public static QueryFilter Equal(string field, IEnumerable<object> values) => new QueryFilter(field, QueryOperators.EQ, values);

        [DebuggerStepThrough]
        public static QueryFilter NotEqual(string field, object value) => new QueryFilter(field, QueryOperators.NOTEQ, value);

        [DebuggerStepThrough]
        public static QueryFilter NotEqual(string field, IEnumerable<object> values) => new QueryFilter(field, QueryOperators.NOTEQ, values);

        [DebuggerStepThrough]
        public static QueryFilter Greather(string field, object value) => new QueryFilter(field, QueryOperators.GREATHER, value);

        [DebuggerStepThrough]
        public static QueryFilter GreatherEqual(string field, object value) => new QueryFilter(field, QueryOperators.GREATHERANDEQUAL, value);

        [DebuggerStepThrough]
        public static QueryFilter Less(string field, object value) => new QueryFilter(field, QueryOperators.LESS, value);

        [DebuggerStepThrough]
        public static QueryFilter LessEqual(string field, object value) => new QueryFilter(field, QueryOperators.LESSANDEQUAL, value);

        [DebuggerStepThrough]
        public static QueryFilter IsNull(string field) => new QueryFilter(field, QueryOperators.ISNULL, null);

        [DebuggerStepThrough]
        public static QueryFilter IsNotNull(string field) => new QueryFilter(field, QueryOperators.ISNOTNULL, null);

        [DebuggerStepThrough]
        public static QueryFilter Single(QueryFilter filter)
        {
            return new QueryFilter()
            {
                Logic = QueryLogicalOperators.AND,
                Filters = new List<QueryFilter> {
                    filter
                }
            };
        }

        [DebuggerStepThrough]
        public QueryFilter(string _field, string _operator, object _Value)
        {

            Field = _field;
            Operator = _operator;
            Value = _Value;
        }

        [DebuggerStepThrough]
        public QueryFilter(string _field, string _operator)
        {

            Field = _field;
            Operator = _operator;
            Value = null;
        }

        [DebuggerStepThrough]
        public QueryFilter(string _field, string _operator, object[] _Values)
        {
            Field = _field;
            Operator = _operator;
            Values = _Values;
        }

        public bool HasInnerFilters()
        {
            return !Filters.IsNullOrEmpty();
        }

        [DebuggerStepThrough]
        public static QueryFilter And(IEnumerable<QueryFilter> filters)
        {
            return new QueryFilter
            {
                Logic = QueryLogicalOperators.AND,
                Filters = filters.ToList()
            };
        }

        [DebuggerStepThrough]
        public static QueryFilter Or(params QueryFilter[] filters)
        {
            return new QueryFilter
            {
                Logic = QueryLogicalOperators.OR,
                Filters = filters.ToList()
            };
        }

        [DebuggerStepThrough]
        public static QueryFilter And(params QueryFilter[] filters)
        {
            return new QueryFilter
            {
                Logic = QueryLogicalOperators.AND,
                Filters = filters.ToList()
            };
        }

        [DebuggerStepThrough]
        public static QueryFilter In<T>(string field, IEnumerable<T> values)
        {
            var queryFilter = new QueryFilter(field, QueryOperators.IN, null)
            {
                Values = values
            };
            return queryFilter;
        }

    }

}
