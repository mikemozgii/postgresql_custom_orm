
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Handlers
{
    public  class  ObjectConverterHandler
    {
        public  string ObjectValueToPostgreString(object value)
        {
            if (value != null)
            {
                var typeName = value.GetType().Name;

                if ((value is ICollection) || typeName.Contains("HashSet"))
                {
                    Type type = value.GetType().GetGenericArguments()[0];
                    typeName = $"{type.Name}[]";
                }


                switch (typeName)
                {


                    case "Guid":
                    case "IPAddress":
                    case "String":
                        value = value.ToString().Replace("'", @"''");
                        return "'" + value + "'";
                    case "String[]":
                    case "Int32[]":
                    case "Int64[]":
                        object obj = value;
                        return "'{" + string.Join(",", obj) + "}'";
                    case "Int64":
                    case "Int32":
                    case "Int16":
                    case "Decimal":
                    case "Float":
                    case "Single":
                    case "Double":
                        return value.ToString();
                    case "Guid[]":
                        var rs = new List<string>();

                        foreach (var r in value as IEnumerable)
                        {
                            rs.Add($"'{(Guid)r}'");
                        }

                        return string.Join(",", rs);
                    case "Boolean":
                        return (bool)value ? "TRUE" : "FALSE";
                    case "DateTime":
                        return "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                    case "TimeSpan":
                        var temp = (TimeSpan)value;
                        var t = new DateTime(2019,01,01).Add(temp);
                        return "'" +  (t.ToString("HH:mm:ss")) + "'";
                    //case "OffsetDateTime":
                    //    return "'" + ((OffsetDateTime)value).ToString() + "'";
                    case "Byte[]":
                        return ByteArrayToString((Byte[])value);
                    default:
                        return "NULL";
                }
            }
            else
            {
                return "NULL";
            }
        }

        public  string ObjectValueToPostgreString_IN(object value)
        {



            if (value != null)
            {
                var typeName = value.GetType().Name;

                if (value is IEnumerable || typeName.Contains("HashSet"))
                {
                    Type type = value.GetType().GetGenericArguments()[0];
                    typeName = $"{type.Name}[]";
                }


                switch (typeName)
                {
                    case "Guid":
                    case "IPAddress":
                    case "String":
                        return "('" + value + "')";
                    case "String[]":
                        var tempValueArray = (string[])value;
                        var tempValue = "";
                        var index = tempValueArray.Length;
                        for (var i = 0; i < index; i++)
                        {
                            if (i != (index - 1))
                            {
                                tempValue += "\'" + tempValueArray[i] + "\',";
                            }
                            else
                            {
                                tempValue += "\'" + tempValueArray[i] + "\'";
                            }
                        }
                        return "(" + tempValue + ")";
                    case "Int32[]":
                        return "(" + String.Join(",", (int[])value) + ")";
                    case "Int64[]":
                        return "(" + String.Join(",", (long[])value) + ")";
                    case "String[][]":
                    case "Int32[][]":
                    case "Int64[][]":
                        var tempValueArray_1 = (int[][])value;
                        var tempValue_1 = "";
                        var index_1 = tempValueArray_1.Length;
                        for (var i = 0; i < index_1; i++)
                        {
                            if (i != (index_1 - 1))
                            {
                                tempValue_1 += "'{" + String.Join(",", (tempValueArray_1[i])) + "}',";
                            }
                            else
                            {
                                tempValue_1 += "'{" + String.Join(",", (tempValueArray_1[i])) + "}'";
                            }
                        }
                        return "(" + tempValue_1 + ")";
                    case "Guid[]":
                        var rs = new List<string>();

                        foreach (var r in value as IEnumerable)
                        {
                            rs.Add($"'{(Guid)r}'");
                        }

                        return $"({string.Join(",", rs)})";
                    case "Int64":
                    case "Int32":
                    case "Int16":
                    case "Decimal":
                    case "Float":
                    case "Single":
                        return "(" + value + ")";
                    case "Boolean":
                        return (bool)value ? "(TRUE)" : "(FALSE)";
                    case "DateTime":
                        return "('" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    case "TimeSpan":
                        var temp = (TimeSpan)value;
                        var t = new DateTime(2019, 01, 01).Add(temp);
                        return "('" + (t.ToString("HH:mm:ss")) + "')";
                    default:
                        return "(NULL)";
                }
            }
            else
            {
                return "NULL";
            }
        }
 
        //'03:00:00'
        //public  T DeserializeObjectFromDataRow<T>(DataRow row) where T : new()
        //{

        //    if (row == null)
        //        return default(T);
        //    var obj = new T();
        //    var columnsNames = (from DataColumn col in row.Table.Columns select col.ColumnName).ToList();
        //    var Properties = typeof(T).GetProperties();
        //    foreach (var objProperty in Properties)
        //    {
        //        var tempColumnName = columnsNames.Find(q => String.Equals(q, objProperty.Name, StringComparison.CurrentCultureIgnoreCase));
        //        if (!String.IsNullOrEmpty(tempColumnName))
        //        {
        //            var value = row[tempColumnName].ToString();
        //            if (!String.IsNullOrEmpty(value))
        //            {

        //                objProperty.SetValue(obj, row[tempColumnName]);


        //            }
        //        }
        //    }
        //    return obj;
        //}

        //public  T DeserializeObjectFromDataRow<T>(DataRow row, List<string> columnsNames) where T : new()
        //{

        //    if (row == null)
        //        return default(T);
        //    var obj = new T();
        //    var Properties = typeof(T).GetProperties();
        //    foreach (var objProperty in Properties)
        //    {
        //        var tempColumnName = columnsNames.Find(q => String.Equals(q, objProperty.Name, StringComparison.CurrentCultureIgnoreCase));
        //        if (!String.IsNullOrEmpty(tempColumnName))
        //        {
        //            var value = row[tempColumnName].ToString();
        //            if (!String.IsNullOrEmpty(value))
        //            {
        //                objProperty.SetValue(obj, row[tempColumnName]);
        //            }
        //        }
        //    }
        //    return obj;
        //}

        //public  List<T> DeserializeObjectFromDataTable<T>(DataTable table) where T : new()
        //{
        //    var tempList = new List<T>();
        //    try
        //    {
        //        var columnsNames = (from DataColumn col in table.Columns select col.ColumnName).ToList();
        //        tempList = table.AsEnumerable().ToList().ConvertAll<T>(row => DeserializeObjectFromDataRow<T>(row, columnsNames));
        //        //foreach (DataRow row in table.Rows)
        //        //{
        //        //   tempList.Add(DeserializeObjectFromDataRow<T>(row, columnsNames));
        //        //}
        //        return tempList;
        //    }
        //    catch
        //    {
        //        return tempList;
        //    }

        //}

        public  string ByteArrayToString(byte[] ba)
        {
            var hex = BitConverter.ToString(ba);
            hex = hex.Replace("-", "");
            return String.Format("decode('{0}', 'hex')", hex);
        }

        public  string ConvertJsonArrayToPostgesJsonArray(object value)
        {
            if (value != null)
            {
                var objType = value.GetType();
                switch (objType.Name)
                {
                    case "String":

                        var temp = value.ToString();
                        if (!String.IsNullOrWhiteSpace(temp))
                        {
                            var tempValue = temp.Replace("\"", "\\\"");
                            tempValue = tempValue.Replace("{", "\"{");
                            tempValue = tempValue.Replace("}", "}\"");
                            tempValue = tempValue.Substring(1);
                            tempValue = tempValue.Remove(tempValue.Length - 1, 1);
                            tempValue = "{" + tempValue + "}";

                            return tempValue;
                        }
                        else
                        {
                            return null;
                        }


                    case "String[]":

                        var temp1 = (string[])value;
                        if (temp1.Any())
                        {

                            var tempValue = String.Join(",", temp1).Replace("\"", "\\\"");
                            tempValue = tempValue.Replace("{", "\"{");
                            tempValue = tempValue.Replace("}", "}\"");
                            tempValue = "{" + tempValue + "}";


                            tempValue = tempValue.Replace(":\"{", ":{");
                            tempValue = tempValue.Replace("}\",\\\"", "},\\\"");


                            return tempValue;
                        }
                        else
                        {
                            return null;
                        }

                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
