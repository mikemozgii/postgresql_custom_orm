namespace UnivIntel.PostgreSQL.ORM.Core.Enums
{
    public static class QueryOperators
    {
        public static readonly string EQ = "=";
        public static readonly string NOTEQ = "<>";
        public static readonly string IN = "IN";
        public static readonly string ISNULL = "IS NULL";
        public static readonly string ISNOTNULL = "IS NOT NULL";
        public static readonly string GREATHER = ">";
        public static readonly string LESS = "<";
        public static readonly string GREATHERANDEQUAL = ">=";
        public static readonly string LESSANDEQUAL = "<=";
        public static readonly string BETWEEN = "BETWEEN";
    }
}
