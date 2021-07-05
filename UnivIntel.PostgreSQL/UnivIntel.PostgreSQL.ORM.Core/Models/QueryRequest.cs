using System;
using System.Collections.Generic;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Models
{
    public class QueryRequest
    {
        public string Sql { get; set; }
        //public string ConnStr { get; set; }
        //public string ApplicationName { get; set; }
        public DateTime Date { get; set; }

        public QueryRequest(string sql)
        {
            Sql = sql;
            Date= DateTime.UtcNow;
            //ConnStr = connStr;
            //ApplicationName = applicationName;
            //SetApplicationName(applicationName);
        }

        //public void SetApplicationName(string applicationName = null)
        //{           
        //    if (!string.IsNullOrWhiteSpace(ApplicationName))
        //    {
        //        ApplicationName = applicationName;
        //        ConnStr = $"{ConnStr};ApplicationName={ApplicationName}";
        //    }
        //}

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(Sql)) return false;

            return true;
        }
    }
}
