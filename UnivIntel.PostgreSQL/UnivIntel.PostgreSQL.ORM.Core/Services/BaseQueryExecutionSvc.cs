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
    public class BaseQueryExecutionSvc
    {
        public string ApplicationName { get; set; }
        public string ConnStr { get; set; }
        public BaseQueryExecutionSvc(string connStr, string applicationName = null)
        {
            ConnStr = connStr;
            ApplicationName = applicationName;
            ProcessConnString();
        }

        public BaseQueryExecutionSvc()
        {
        }

        public void ProccessResults<T>(IEnumerable<T> results) where T : class
        {
            var type = typeof(T);

            foreach (var r in results)
            {
                MethodInfo method = type.GetMethod("FillEntityPrimaryKey");
                method.Invoke(r, null);
            }
        }
        public string ProcessConnString(string connString)
        {
            if (!string.IsNullOrWhiteSpace(ApplicationName))
            {
                return $"{connString};ApplicationName={ApplicationName}";
            }
            return connString;
        }

        public void ProcessConnString()
        {
            if (!string.IsNullOrWhiteSpace(ApplicationName))
            {
                ConnStr = $"{ConnStr};ApplicationName={ApplicationName}";
            }
        }

        //public void SetApplicationName(string appName)
        //{
        //    if (string.IsNullOrWhiteSpace(appName))
        //    {
        //        ApplicationName = appName;
        //    }
        //}
    }
}
