using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace UnivIntel.PostgreSQL.ORM.Core.Handlers
{
    public static class AuditConfigHdr
    {
        public static string Db_Server = "192.168.1.238";
        public static string Db_Name = "audit";
        public static string Db_UserId = "postgres";
        public static string Db_Password = "admin32";
        public static bool IsAudit = true;
        public static string ConnStr { get; set; } = $"Host={Db_Server};Database={Db_Name};Username={Db_UserId};Password={Db_Password}";
    }
}
