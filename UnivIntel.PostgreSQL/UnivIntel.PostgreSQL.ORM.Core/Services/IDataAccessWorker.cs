using System;
using System.Collections.Generic;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Services
{
   public interface IDataAccessWorker
    {
        IQueryExecutionSvc _QueryExecutionSvc { get; set; }
    }
}
