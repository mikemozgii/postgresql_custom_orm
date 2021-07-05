using UnivIntel.PostgreSQL.ORM.Core;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;
using UnivIntel.PostgreSQL.ORM.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Entities
{
    [PgsDataTable("TableColumnAudits")]
    public class e_TableColumnAudit : BaseRepository<e_TableColumnAudit>
    {
        public e_TableColumnAudit(IQueryExecutionSvc _queryExecutionSvc) 
        {
        }

        [PgsPK]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public int ActionTypeId { get; set; }
        public string OriginalSql { get; set; }
        public Guid EntityId { get; set; }
        public Guid AccountId { get; set; }
        public string Table { get; set; }
        public string Column { get; set; }
        public string Value { get; set; }
    }
}