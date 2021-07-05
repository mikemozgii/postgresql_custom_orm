using UnivIntel.PostgreSQL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Models
{
    public class QueryResponseGeneric<T> where T : class
    {
        //public string Sql { get; set; }
        public TimeSpan Time { get; set; }
        public bool Success { get; set; }
        public bool IsSingleOrDefault { get { return Results?.Count() == 1; } }
        public bool HasResults { get { return !Results.IsNullOrEmpty(); } }
        public DateTime DateCompleted { get; set; }
        public virtual IEnumerable<T> Results { get; set; }
        public int Count { get { return HasResults ? Results.Count() : 0; } }
        public virtual T FirstOrDefault { get { return Results?.FirstOrDefault(); } }
        public virtual T First { get { return Results.First(); } }
        public void Map(QueryResponse model)
        {
            //Sql = model.Sql;
            Time = model.Time;
            Success = model.Success;
            DateCompleted = model.DateCompleted;
        }


        public bool TryGetFirstOrDefault(out T firstOrDefault)
        {
            firstOrDefault = FirstOrDefault;
            if (FirstOrDefault != null)
            {
                return true;
            }
            return false;
        }

        public bool TryGetResults(out IEnumerable<T> results)
        {
            results = Results;
            if (!results.IsNullOrEmpty())
            {
                return true;
            }
            return false;
        }

        //public void Map(QueryResponse model, DateTime dateCompleted)
        //{
        //    Sql = model.Sql;
        //    Time = model.Time;
        //    Success = model.Success;
        //    DateCompleted = dateCompleted;
        //}
    }
}
