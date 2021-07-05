using UnivIntel.PostgreSQL.Core;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;
using UnivIntel.PostgreSQL.ORM.Core.Handlers;
using UnivIntel.PostgreSQL.ORM.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Models
{
    public class DeleteQuery<T> : BaseQueryGeneric<T> where T: class
    {
        public DeleteQuery(QueryFilter _filter) 
        {
            Request = new QueryRequest(new QueryBuilderHandler().DELETE(GetTableNameFromAttribute(), _filter));
        }
    }
}
