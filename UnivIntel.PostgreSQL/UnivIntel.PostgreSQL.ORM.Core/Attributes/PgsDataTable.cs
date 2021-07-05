using System;
using System.Collections.Generic;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class PgsDataTable : System.Attribute
    {
        public string Name;
        public PgsDataTable(string name)
        {
            Name = name;
        }
    }
}
