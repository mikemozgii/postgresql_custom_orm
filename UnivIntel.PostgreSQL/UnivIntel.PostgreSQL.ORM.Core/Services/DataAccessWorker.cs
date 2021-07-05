using System;
using System.Collections.Generic;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Services
{
    public class DataAccessWorker : IDataAccessWorker
    {
        public IQueryExecutionSvc _QueryExecutionSvc { get; set; }

        public DataAccessWorker(IQueryExecutionSvc queryExecutionSvc)
        {
            _QueryExecutionSvc = queryExecutionSvc;
        }
    }
}
