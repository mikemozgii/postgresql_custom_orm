using UnivIntel.PostgreSQL.Core;
using UnivIntel.PostgreSQL.ORM.Core.Entities;
using UnivIntel.PostgreSQL.ORM.Core.Enums;
using UnivIntel.PostgreSQL.ORM.Core.Handlers;
using UnivIntel.PostgreSQL.ORM.Core.Uuid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Models
{
    public class SqlDetails
    {
        public string Sql { get; set; }
        public string TableName { get; set; }
        public int ActionType { get; set; }
        public Guid EntityId { get; set; }
        public Guid AccountId { get; set; }
        public bool IsAllowForChangeAudit { get {
                return ActionType == QueryActionTypes.INSERT || ActionType == QueryActionTypes.UPDATE || ActionType == QueryActionTypes.DELETE;
            } }


        public IDictionary<string, string> ColumnValues { get; set; } = new Dictionary<string, string>();

        public SqlDetails(string sql, string tableName, int actionType , string[] columnNames, object[] values)
        {
            Sql = sql;
            TableName = tableName;
            ActionType = actionType;
            for (var i = 0; i < columnNames.Count(); i++)
            {
                ColumnValues.Add(columnNames[i], new ObjectConverterHandler().ObjectValueToPostgreString(values[i]));
            }
        }

        public SqlDetails(string sql, string tableName, int actionType)
        {
            Sql = sql;
            TableName = tableName;
            ActionType = actionType;
        }

        public SqlDetails(string sql, string tableName, string[] columnNames, string[] values)
        {
            Sql = sql;
            TableName = tableName;

            for (var i = 0; i < columnNames.Count(); i++)
            {
                ColumnValues.Add(columnNames[i], values[i]);
            }
        }

        //public List<e_TableColumnAudit> ProcessAudit()
        //{
        //    var rs = new List<e_TableColumnAudit>();
        //    var dtn = DateTime.UtcNow;
        //    var pluuidSvc = new PKuuidSvc();

        //    if (ActionType == QueryActionTypes.DELETE)
        //    {
        //        var r = new e_TableColumnAudit()
        //        {
        //            Id = pluuidSvc.GenerateGuid(),
        //            Date = dtn,
        //            ActionTypeId = ActionType,
        //            OriginalSql = Sql,
        //            Table = TableName,
        //            Column = "*",
        //            Value = null,
        //            EntityId = EntityId,
        //            AccountId = AccountId
        //        };
        //        rs.Add(r);
        //    }
        //    else
        //    {
        //        foreach (var item in ColumnValues)
        //        {
        //            var r = new e_TableColumnAudit()
        //            {
        //                Date = dtn,
        //                ActionTypeId = ActionType,
        //                OriginalSql = Sql,
        //                Table = TableName,
        //                Column = item.Key,
        //                Value = item.Value,
        //                EntityId = EntityId,
        //                AccountId = AccountId
        //            };
        //            rs.Add(r);
        //        }

        //    }



        //    return rs;
        //}

        //public string ProcessAuditSql()
        //{
        //    var rs = ProcessAudit();



       
        //    if (rs.IsNullOrEmpty()) return null;
        //    var query = "";
        //    foreach (var r in rs)
        //    {
        //        query += r.INSERT();
        //    }
        //    return query;
        //}
    }
}
