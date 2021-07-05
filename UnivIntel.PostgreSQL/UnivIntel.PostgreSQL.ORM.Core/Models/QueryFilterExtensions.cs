using System.Collections.Generic;
using System.Diagnostics;

namespace UnivIntel.PostgreSQL.ORM.Core.Models
{
    public static class QueryFilterExtensions
    {

        [DebuggerStepThrough]
        public static QueryFilter And(this QueryFilter queryFilter, params QueryFilter[] filters)
        {
            queryFilter.Filters.Add(QueryFilter.And(filters));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter Or(this QueryFilter queryFilter, params QueryFilter[] filters)
        {
            queryFilter.Filters.Add(QueryFilter.Or(filters));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter Eq(this QueryFilter queryFilter, string field, object value)
        {
            queryFilter.Filters.Add(QueryFilter.Equal(field, value));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter Eq(this QueryFilter queryFilter, string field, IEnumerable<object> values)
        {
            queryFilter.Filters.Add(QueryFilter.Equal(field, values));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter Ne(this QueryFilter queryFilter, string field, object value)
        {
            queryFilter.Filters.Add(QueryFilter.NotEqual(field, value));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter Ne(this QueryFilter queryFilter, string field, IEnumerable<object> values)
        {
            queryFilter.Filters.Add(QueryFilter.NotEqual(field, values));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter Gt(this QueryFilter queryFilter, string field, object value)
        {
            queryFilter.Filters.Add(QueryFilter.Greather(field, value));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter Gte(this QueryFilter queryFilter, string field, IEnumerable<object> values)
        {
            queryFilter.Filters.Add(QueryFilter.GreatherEqual(field, values));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter Lt(this QueryFilter queryFilter, string field, object value)
        {
            queryFilter.Filters.Add(QueryFilter.Greather(field, value));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter Lte(this QueryFilter queryFilter, string field, IEnumerable<object> values)
        {
            queryFilter.Filters.Add(QueryFilter.GreatherEqual(field, values));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter EqNull(this QueryFilter queryFilter, string field, object value)
        {
            queryFilter.Filters.Add(QueryFilter.Greather(field, value));

            return queryFilter;
        }

        [DebuggerStepThrough]
        public static QueryFilter NeNull(this QueryFilter queryFilter, string field, IEnumerable<object> values)
        {
            queryFilter.Filters.Add(QueryFilter.GreatherEqual(field, values));

            return queryFilter;
        }

    }

}
