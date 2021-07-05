using Dapper;
using UnivIntel.PostgreSQL.Core;
using UnivIntel.PostgreSQL.ORM.Core.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnivIntel.PostgreSQL.ORM.Core.Services
{
    public class QueryExecutionSvc : BaseQueryExecutionSvc, IQueryExecutionSvc
    {


        public QueryExecutionSvc(string connStr, string applicationName = null) : base(connStr, applicationName)
        {
        }

        public QueryExecutionSvc()
        {
        }


        public QueryResponse ExecuteResultQuery(params QueryRequest[] request)
        {
            var queryResponse = new QueryResponse();
            //queryResponse.Sql = request.Sql;
            //if (req?.IsValid() != true) return queryResponse;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var sql = string.Join("", request.Select(q => q.Sql));
                using (var conn = new NpgsqlConnection(ProcessConnString(ConnStr)))
                {
                    queryResponse.Results = conn.Query<object>(sql);
                }
                queryResponse.Success = true;
            }
            catch
            {
                queryResponse.Success = false;
                throw;
            }
            finally
            {
                if (queryResponse.Results.IsNullOrEmpty()) queryResponse.Results = Enumerable.Empty<object>();

                stopwatch.Stop();
                queryResponse.Time = stopwatch.Elapsed;
                queryResponse.DateCompleted = DateTime.Now;
            }

            return queryResponse;
        }

        public async Task<QueryResponseGeneric<T>> ExecuteResultQueryAsync<T>(params QueryRequest[] request) where T : class
        {
            var queryResponse = new QueryResponseGeneric<T>();
            //if (req?.IsValid() != true) return queryResponse;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var sql = string.Join("", request.Select(q => q.Sql));
                using (var conn = new NpgsqlConnection(ProcessConnString(ConnStr)))
                {
                    queryResponse.Results = await conn.QueryAsync<T>(sql);
                    ProccessResults<T>(queryResponse.Results);
                }
                queryResponse.Success = true;

            }
            catch
            {
                queryResponse.Success = false;
                throw;
            }
            finally
            {
                if (queryResponse.Results.IsNullOrEmpty())
                {
                    queryResponse.Results = Enumerable.Empty<T>();
                }
                stopwatch.Stop();
                queryResponse.Time = stopwatch.Elapsed;
                queryResponse.DateCompleted = DateTime.Now;
            }
            return queryResponse;
        }
        public QueryResponseGeneric<T> ExecuteResultQuery<T>(params QueryRequest[] request) where T : class
        {
            var queryResponse = new QueryResponseGeneric<T>();
            //if (req?.IsValid() != true) return queryResponse;


            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var sql = string.Join("", request.Select(q => q.Sql));
                using (var conn = new NpgsqlConnection(ProcessConnString(ConnStr)))
                {
                    queryResponse.Results = conn.Query<T>(sql);
                    ProccessResults<T>(queryResponse.Results);

                }
                queryResponse.Success = true;

            }
            catch
            {
                queryResponse.Success = false;
                throw;
            }
            finally
            {
                if (queryResponse.Results.IsNullOrEmpty()) queryResponse.Results = Enumerable.Empty<T>();

                stopwatch.Stop();
                queryResponse.Time = stopwatch.Elapsed;
                queryResponse.DateCompleted = DateTime.Now;
            }
            return queryResponse;
        }

        //public async Task<QueryResponse> ExecuteResultQueryAsync()
        //{
        //    var queryResponse = new QueryResponse();
        //    queryResponse.Sql = Request.Sql;
        //    //if (req?.IsValid() != true) return queryResponse;

        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    try
        //    {
        //        using (var conn = new NpgsqlConnection(ProcessConnString(ConnStr)))
        //        {
        //            queryResponse.Results = await conn.QueryAsync<object>(Request.Sql);
        //        }
        //        queryResponse.Success = true;
        //    }
        //    catch
        //    {
        //        queryResponse.Success = false;
        //        throw;
        //    }
        //    finally
        //    {
        //        if (queryResponse.Results.IsNullOrEmpty())
        //        {
        //            queryResponse.Results = Enumerable.Empty<object>();
        //        }
        //        stopwatch.Stop();
        //        queryResponse.Time = stopwatch.Elapsed;
        //        queryResponse.DateCompleted = DateTime.Now;
        //    }
        //    return queryResponse;
        //}

        public QueryResponse ExecuteQuery(params QueryRequest[] request)
        {
            var queryResponse = new QueryResponse();

            //if (req?.IsValid() != true) return queryResponse;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var sql = string.Join("", request.Select(q => q.Sql));
                using (var conn = new NpgsqlConnection(ProcessConnString(ConnStr)))
                {
                    conn.Execute(sql);
                }
                queryResponse.Results = Enumerable.Empty<object>();
                queryResponse.Success = true;
            }
            catch
            {
                queryResponse.Success = false;
                throw;
            }
            finally
            {
                if (queryResponse.Results.IsNullOrEmpty())
                {
                    queryResponse.Results = Enumerable.Empty<object>();
                }
                stopwatch.Stop();
                queryResponse.Time = stopwatch.Elapsed;
                queryResponse.DateCompleted = DateTime.Now;
            }
            return queryResponse;
        }
        public async Task<QueryResponse> ExecuteQueryAsync(params QueryRequest[] request)
        {
            var queryResponse = new QueryResponse();
            //queryResponse.Sql = request.Sql;
            //if (req?.IsValid() != true) return queryResponse;

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var sql = string.Join("", request.Select(q => q.Sql));
                using (var conn = new NpgsqlConnection(ProcessConnString(ConnStr)))
                {
                    await conn.ExecuteAsync(sql);
                }
                queryResponse.Results = Enumerable.Empty<object>();
                queryResponse.Success = true;
            }
            catch
            {
                queryResponse.Success = false;
                throw;
            }
            finally
            {
                if (queryResponse.Results.IsNullOrEmpty())
                {
                    queryResponse.Results = Enumerable.Empty<object>();
                }
                stopwatch.Stop();
                queryResponse.Time = stopwatch.Elapsed;
                queryResponse.DateCompleted = DateTime.Now;
            }

            return queryResponse;

        }


        public QueryResponseGeneric<T> SELECTALL<T>() where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(new T().SELECTALL());
        }

        public async Task<QueryResponseGeneric<T>> SELECTALLAsync<T>() where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(new T().SELECTALL());
        }

        public QueryResponseGeneric<T> SELECTbyId<T>(object id) where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(new T().SELECTbyId(id));
        }

        public async Task<QueryResponseGeneric<T>> SELECTbyIdAsync<T>(object id) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(new T().SELECTbyId(id));
        }

        public QueryResponseGeneric<T> SELECTbyColumnValue<T>(params KeyValuePair<string, object>[] vals) where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(new T().SELECTbyColumnValue(vals));
        }

        public async Task<QueryResponseGeneric<T>> SELECTbyColumnValueAsync<T>(params KeyValuePair<string, object>[] vals) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(new T().SELECTbyColumnValue(vals));
        }


        public QueryResponseGeneric<T> SELECT<T>(QueryFilter filter) where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(new T().SELECT(filter));
        }

        public async Task<QueryResponseGeneric<T>> SELECTAsync<T>(QueryFilter filter) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(new T().SELECT(filter));
        }

        public QueryResponseGeneric<T> INSERT<T>(T value) where T : BaseRepository<T>
        {
            return ExecuteResultQuery<T>(value.INSERT());
        }

        public async Task<QueryResponseGeneric<T>> INSERTAsync<T>(T value) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(value.INSERT());
        }

        public QueryResponseGeneric<T> UPDATE<T>(T value, QueryFilter queryFilter = null) where T : BaseRepository<T>
        {
            return ExecuteResultQuery<T>(value.UPDATE(queryFilter));
        }

        public async Task<QueryResponseGeneric<T>> UPDATEAsync<T>(T value, QueryFilter queryFilter = null) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(value.UPDATE(queryFilter));
        }



        public QueryResponseGeneric<T> UPDATE<T>(QueryFilter filter = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(new T().UPDATE(filter, _vals));
        }

        public async Task<QueryResponseGeneric<T>> UPDATEAsync<T>(QueryFilter filter = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(new T().UPDATE(filter, _vals));
        }


        public QueryResponseGeneric<T> UPDATE<T>(object id = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(new T().UPDATE(id, _vals));
        }

        public async Task<QueryResponseGeneric<T>> UPDATEAsync<T>(object id = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(new T().UPDATE(id, _vals));
        }

        public QueryResponseGeneric<T> DELETE<T>(object id) where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(new T().DELETE(id));
        }

        public async Task<QueryResponseGeneric<T>> DELETEAsync<T>(object id) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(new T().DELETE(id));
        }

        public QueryResponseGeneric<T> DELETE<T>(T value) where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(value.DELETE());
        }

        public async Task<QueryResponseGeneric<T>> DELETEAsync<T>(T value) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(value.DELETE());
        }

        public QueryResponseGeneric<T> DELETE<T>(QueryFilter queryFilter) where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(new T().DELETE(queryFilter));
        }

        public async Task<QueryResponseGeneric<T>> DELETEAsync<T>(QueryFilter queryFilter) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(new T().DELETE(queryFilter));
        }


        public QueryResponseGeneric<T> UPSERT<T>(QueryFilter filter = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new()
        {
            return ExecuteResultQuery<T>(new T().UPDATE(filter, _vals));
        }

        public async Task<QueryResponseGeneric<T>> UPSERTsync<T>(QueryFilter filter = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new()
        {
            return await ExecuteResultQueryAsync<T>(new T().UPDATE(filter, _vals));
        }









        //public QueryResponse ExecuteQuery(string sql, string connStr)
        //{
        //    return ExecuteQuery(new QueryRequest(sql, connStr));
        //}
        //public async Task<QueryResponse> ExecuteQueryAsync(string sql, string connStr)
        //{
        //    return await ExecuteQueryAsync(new QueryRequest(sql, connStr));
        //}

    }
}
