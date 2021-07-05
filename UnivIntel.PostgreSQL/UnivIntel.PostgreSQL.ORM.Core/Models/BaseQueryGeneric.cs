using Dapper;
using UnivIntel.PostgreSQL.Core;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;
using UnivIntel.PostgreSQL.ORM.Core.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnivIntel.PostgreSQL.ORM.Core.Models
{
    public class BaseQueryGeneric<T> where T : class
    {

        public bool ExpectResults { get; set; }
        //public bool ExpectResults { get; set; }
        public QueryRequest Request { get; set; }
        //public QueryResponseGeneric<T> Response { get; set; }
        //public QueryFilter Filter { get; set; }

        public BaseQueryGeneric(string sql = null)
        {
            Request = new QueryRequest(sql);
            //Filter = filter;
            //IsValid = IsValid_BaseRequest();
        }

        //public BaseQueryGeneric(QueryFilter filter = null)
        //{
        //    QueryExecutionSvc = _queryExecutionSvc;
        //    Response = new QueryResponseGeneric<T>();
        //    Filter = filter;
        //    //IsValid = IsValid_BaseRequest();
        //}

        //public bool IsValid_BaseRequest()
        //{
        //    return !string.IsNullOrWhiteSpace(QueryExecutionSvc.Sql) && !string.IsNullOrWhiteSpace(Request.ConnStr);
        //}

        //public BaseQueryGeneric<T> Execute()
        //{
        //    if (ExpectResults)
        //    {
        //        Response = QueryExecutionSvc.ExecuteResultQuery<T>();
        //    }
        //    else
        //    {
        //        Response.Map(QueryExecutionSvc.ExecuteQuery());
        //    }

        //    Response.DateCompleted = DateTime.UtcNow;

        //    return this;
        //}

        //public async Task<BaseQueryGeneric<T>> ExecuteAsync()
        //{
        //    if (ExpectResults)
        //    {
        //        Response = await QueryExecutionSvc.ExecuteResultQueryAsync<T>();
        //    }
        //    else
        //    {
        //        Response.Map(await QueryExecutionSvc.ExecuteQueryAsync());
        //    }
        //    return this;
        //}


        public string GetTableNameFromAttribute()
        {           
            if (typeof(T).GetCustomAttributes(typeof(PgsDataTable), true).FirstOrDefault() is PgsDataTable tableNameAttribute)
            {
                return tableNameAttribute.Name;
            }
            return null;
        }


    }
}
