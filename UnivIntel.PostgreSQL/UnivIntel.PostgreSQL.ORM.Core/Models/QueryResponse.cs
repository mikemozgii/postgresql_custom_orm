using System;
using System.Collections.Generic;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Models
{
    public class QueryResponse
    {
        public string Sql { get; set; }
        public TimeSpan Time { get; set; }
        public bool Success { get; set; }
        public DateTime DateCompleted { get; set; }
        public virtual IEnumerable<object> Results { get; set; }
    }
}
