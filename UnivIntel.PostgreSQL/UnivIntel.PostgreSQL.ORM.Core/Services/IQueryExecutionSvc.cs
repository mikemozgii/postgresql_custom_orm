using System.Collections.Generic;
using System.Threading.Tasks;
using UnivIntel.PostgreSQL.ORM.Core.Models;

namespace UnivIntel.PostgreSQL.ORM.Core.Services
{
    public interface IQueryExecutionSvc
    {
        QueryResponseGeneric<T> DELETE<T>(object id) where T : BaseRepository<T>, new();
        QueryResponseGeneric<T> DELETE<T>(QueryFilter queryFilter) where T : BaseRepository<T>, new();
        QueryResponseGeneric<T> DELETE<T>(T value) where T : BaseRepository<T>, new();
        Task<QueryResponseGeneric<T>> DELETEAsync<T>(object id) where T : BaseRepository<T>, new();
        Task<QueryResponseGeneric<T>> DELETEAsync<T>(QueryFilter queryFilter) where T : BaseRepository<T>, new();
        Task<QueryResponseGeneric<T>> DELETEAsync<T>(T value) where T : BaseRepository<T>, new();
        QueryResponse ExecuteQuery(params QueryRequest[] request);
        Task<QueryResponse> ExecuteQueryAsync(params QueryRequest[] request);
        QueryResponse ExecuteResultQuery(params QueryRequest[] request);
        QueryResponseGeneric<T> ExecuteResultQuery<T>(params QueryRequest[] request) where T : class;
        Task<QueryResponseGeneric<T>> ExecuteResultQueryAsync<T>(params QueryRequest[] request) where T : class;
        QueryResponseGeneric<T> INSERT<T>(T value) where T : BaseRepository<T>;
        Task<QueryResponseGeneric<T>> INSERTAsync<T>(T value) where T : BaseRepository<T>, new();
        QueryResponseGeneric<T> SELECT<T>(QueryFilter filter) where T : BaseRepository<T>, new();
        QueryResponseGeneric<T> SELECTALL<T>() where T : BaseRepository<T>, new();
        Task<QueryResponseGeneric<T>> SELECTALLAsync<T>() where T : BaseRepository<T>, new();
        Task<QueryResponseGeneric<T>> SELECTAsync<T>(QueryFilter filter) where T : BaseRepository<T>, new();
        QueryResponseGeneric<T> SELECTbyColumnValue<T>(params KeyValuePair<string, object>[] vals) where T : BaseRepository<T>, new();
        Task<QueryResponseGeneric<T>> SELECTbyColumnValueAsync<T>(params KeyValuePair<string, object>[] vals) where T : BaseRepository<T>, new();
        QueryResponseGeneric<T> SELECTbyId<T>(object id) where T : BaseRepository<T>, new();
        Task<QueryResponseGeneric<T>> SELECTbyIdAsync<T>(object id) where T : BaseRepository<T>, new();
        QueryResponseGeneric<T> UPDATE<T>(object id = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new();
        QueryResponseGeneric<T> UPDATE<T>(QueryFilter filter = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new();
        QueryResponseGeneric<T> UPDATE<T>(T value, QueryFilter queryFilter = null) where T : BaseRepository<T>;
        Task<QueryResponseGeneric<T>> UPDATEAsync<T>(object id = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new();
        Task<QueryResponseGeneric<T>> UPDATEAsync<T>(QueryFilter filter = null, params KeyValuePair<string, object>[] _vals) where T : BaseRepository<T>, new();
        Task<QueryResponseGeneric<T>> UPDATEAsync<T>(T value, QueryFilter queryFilter = null) where T : BaseRepository<T>, new();
    }
}