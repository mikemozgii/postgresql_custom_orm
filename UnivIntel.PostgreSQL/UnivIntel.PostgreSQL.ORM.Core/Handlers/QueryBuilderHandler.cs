using UnivIntel.PostgreSQL.Core;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;
using UnivIntel.PostgreSQL.ORM.Core.Enums;
using UnivIntel.PostgreSQL.ORM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Handlers
{
    public class QueryBuilderHandler
    {
        //public  string[] ColumnExceptionName = { "e_", "v_", "j_" };

        //public  bool IsValidColumnName(string columnName)
        //{
        //    return ColumnExceptionName.All(val => !columnName.StartsWith(val));
        //}

        //public  string ProcessFields(string[] fields)
        //{
        //    if (fields.IsNullOrEmpty()) return null;
        //    fields = fields.Where(q => !q.Contains("*")).ToArray();
        //    if (fields.IsNullOrEmpty()) return null;
        //    for (var i = 0; i < fields.Length; i++)
        //    {
        //            fields[i] = "\"" + fields[i] + "\"";       
        //    }

        //    return String.Join(",", fields);
        //}
        //public  string ProcessFieldsOrRetunAll(string[] fields)
        //{

        //    return ProcessFields(fields) ?? "*";
        //}



        //public  string qValuesToCommand(string values)
        //{
        //    return $"{QueryCommands.VALUES}({values})";
        //}
        public  string qValues(object[] values)
        {
            var tempArray = new string[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                tempArray[i] = new ObjectConverterHandler().ObjectValueToPostgreString(values[i]);
            }

            var r = String.Join(", ", tempArray);

            //if (proccessFurther)
            //{
            //    r = qValuesToCommand(r);
            //}

            return r;
        }

        public string qValues(KeyValuePair<string, object>[] vals)
        {
            return qValues(vals.Select(q=>q.Value).ToArray());
        }

        public  object[] qValues_Obj(object obj)
        {
            var values = new List<string>();
            var properties = obj.GetType().GetProperties();
            foreach (var objProperty in properties)
            {
                if (!Attribute.IsDefined(objProperty, typeof(PgsNotMapped)))
                {

                    var val = objProperty.GetValue(obj, null);
                    values.Add(new ObjectConverterHandler().ObjectValueToPostgreString(val));
                }             
            }
            return values.ToArray();
        }

        public  IEnumerable<object[]> qValuesWithoutProcessing(IEnumerable<object> valueGroups)
        {
            return valueGroups.Select(a => a.GetType().GetProperties().Where(property =>!Attribute.IsDefined(property, typeof(PgsNotMapped))).Select(property => property.GetValue(a, null)).ToArray()).ToList();
        }

        public  IEnumerable<object[]> qValues_Obj(IEnumerable<object> values)
        {
            var tempArray = new List<object[]>();

            foreach (var v in values)
            {
                tempArray.Add(qValues_Obj(v));
            }
            return tempArray;
        }
        public  string qValues(IEnumerable<object[]> values)
        {
            var tvalues = values.ToList();
            var tempSQL = new string[tvalues.Count];
            for (var i = 0; i < tvalues.Count; i++)
            {
                tempSQL[i] = $"({qValues(tvalues[i])})";
            }

            return string.Join(", ", tempSQL);
        }
        public  string qColumnNames(string[] columnNames)
        {
            return string.Join(", ", columnNames.Select(a => "\"" + a + "\""));
        }

        public string qColumnNames(KeyValuePair<string, object>[] _vals)
        {
            return string.Join(", ", _vals.Select(a => "\"" + a.Key+ "\""));
        }

        public KeyValuePair<string, object>[] qColumnNames_Values_Obj(object obj)
        {
            var cls = qColumnNames_Obj(obj);
            var vls = qValues_Obj(obj);

            var rs = new List <KeyValuePair<string, object>>();
            for (var i=0; i < cls.Length; i++)
            {
                rs.Add(new KeyValuePair<string, object>(cls[i], vls[i]));
            }

            return rs.ToArray();
        }

        public  string[] qColumnNames_Obj(object obj)
        {
            var columnNames = new List<string>();
            var properties = obj.GetType().GetProperties();
            foreach (var objProperty in properties)
            {
                if(!Attribute.IsDefined(objProperty, typeof(PgsNotMapped)))
                {
                    columnNames.Add(objProperty.Name);
                }  
            }

            return columnNames.ToArray();
        }
        public  string[] qColumnNames_Obj(IEnumerable<object> obj)
        {
            return qColumnNames_Obj(obj.First());
        }
        public  string qBrackets(string str)
        {
            return $"{QueryCommands.LEFTBRACKET}{str}{QueryCommands.RIGHTtBRACKET}";
        }
        public  string qFrom(string tableName)
        {
            return $"{QueryCommands.FROM} \"{tableName}\"";
        }
        public  string qInsertInto(string tableName)
        {
            return $"{QueryCommands.INSERT} {QueryCommands.INTO} \"{tableName}\"";
        }
        public  string qUpdate(string tableName)
        {
            return $"{QueryCommands.UPDATE} \"{tableName}\"";
        }
        public  string qOrderField(string field, bool descending = false)
        {
            return $"\"{field}\" {(descending ? QueryCommands.DESCENDING : QueryCommands.ASCENDING)}";
        }
        public  string qOrder(IEnumerable<SelectOrderField> selectOrderFields)
        {
            if (selectOrderFields == null || !selectOrderFields.Any()) return "";

            return $"{QueryCommands.ORDER} {string.Join(",", selectOrderFields.Select(a => qOrderField(a.Field, a.Descending)))}";
        }

        public  string qSet(bool isConvertable = true, params KeyValuePair<string, object>[] vals)
        {
            var tempSQL = new string[vals.Length];
            for (var i = 0; i < vals.Length; i++)
            {
                if (isConvertable)
                {
                    tempSQL[i] = $"\"{vals[i].Key}\" {QueryOperators.EQ} {new ObjectConverterHandler().ObjectValueToPostgreString(vals[i].Value)}";
                }
                else
                {
                    tempSQL[i] = $"\"{vals[i].Key}\" {QueryOperators.EQ} {vals[i].Value}";
                }
                
            }

            return string.Join(", ", tempSQL);
        }
        public string qSet(KeyValuePair<string, object> val)
        {
            return qSet(true, val);
        }
        public  string qSemicolon(bool Include_Semicolon)
        {
            return Include_Semicolon ? QueryCommands.SEMICOLON : "";
        }

        public  string qLimit(long? count)
        {
            if (!count.HasValue) return "";

            return $"{QueryCommands.LIMIT} {count.Value}";
        }

        public  string qOffset(long? count)
        {
            if (!count.HasValue) return "";

            return $"{QueryCommands.OFFSET} {count.Value}";
        }

        public  string SELECT(string tableName, string[] fields, QueryFilter filter, bool Include_Semicolon = true, SelectQueryOptions options = default(SelectQueryOptions))
        {
            var f = new QueryFilterHandler().ProccessFilter(filter);
            if (string.IsNullOrWhiteSpace(f)) return null;
            return $"{QueryCommands.SELECT} {qColumnNames(fields)} {qFrom(tableName)} {f} {qOrder(options?.OrderFields)} {qLimit(options?.Take)} {qOffset(options?.Skip)} {qSemicolon(Include_Semicolon)}";
        }
        public  string SELECT(string tableName, object obj, QueryFilter filter, bool Include_Semicolon = true, SelectQueryOptions options = default(SelectQueryOptions))
        {
            var filters = new QueryFilterHandler().ProccessFilter(filter) ?? "";

            return $"{QueryCommands.SELECT} {qColumnNames(qColumnNames_Obj(obj))} {qFrom(tableName)} {filters} {qOrder(options?.OrderFields)} {qLimit(options?.Take)} {qOffset(options?.Skip)} {qSemicolon(Include_Semicolon)}";
        }
        public  string SELECT(string tableName, QueryFilter filter, bool Include_Semicolon = true, SelectQueryOptions options = default(SelectQueryOptions))
        {
            var f = new QueryFilterHandler().ProccessFilter(filter);
            if (string.IsNullOrWhiteSpace(f)) return null;
            return $"{QueryCommands.SELECT} {QueryCommands.ASTERISK} {qFrom(tableName)} {f} {qOrder(options?.OrderFields)} {qLimit(options?.Take)} {qOffset(options?.Skip)} {qSemicolon(Include_Semicolon)}";
        }


        public  string DELETE_All(string tableName, bool Include_Semicolon = true)
        {
            return $"{QueryCommands.DELETE} {qFrom(tableName)} {qSemicolon(Include_Semicolon)}";
        }
        public  string DELETE(string tableName, QueryFilter filter, bool Include_Semicolon = true)
        {
            var f = new QueryFilterHandler().ProccessFilter(filter);
            if (string.IsNullOrWhiteSpace(f)) return null;
            return $"{QueryCommands.DELETE} {qFrom(tableName)} {f} {qSemicolon(Include_Semicolon)}";
        }





        public  string INSERT(string tableName, object[] values, bool Include_Semicolon = true)
        {
            return $"{qInsertInto(tableName)} {QueryCommands.VALUES} {qBrackets(qValues(values))} {qSemicolon(Include_Semicolon)}";
        }
        public  string INSERT(string tableName, bool Include_Semicolon = true, params KeyValuePair<string, object>[] vals)
        {
            return $"{qInsertInto(tableName)} {qBrackets(qColumnNames(vals))}  {QueryCommands.VALUES} {qBrackets(qValues(vals))} {qSemicolon(Include_Semicolon)}";
        }

        public string INSERT(string tableName, params KeyValuePair<string, object>[] vals)
        {
            return $"{qInsertInto(tableName)} {qBrackets(qColumnNames(vals))}  {QueryCommands.VALUES} {qBrackets(qValues(vals))} {qSemicolon(true)}";
        }

        public  string INSERT(string tableName, IEnumerable<object[]> values, bool Include_Semicolon = true)
        {
            return $"{qInsertInto(tableName)} {QueryCommands.VALUES} {qBrackets(qValues(values))} {qSemicolon(Include_Semicolon)}";
        }
        public  string INSERT(string tableName, string[] columnNames, IEnumerable<object[]> values, bool Include_Semicolon = true)
        {
            return $"{qInsertInto(tableName)} {qBrackets(qColumnNames(columnNames))} {QueryCommands.VALUES} {qValues(values)} {qSemicolon(Include_Semicolon)}";
        }
        //TODO: Add Insert If Differn Objects
        public  string INSERT(string tableName, object obj, bool Include_Semicolon = true)
        {
            return INSERT(tableName, Include_Semicolon, qColumnNames_Values_Obj(obj));
        }
        public  string INSERT(string tableName, IEnumerable<object> obj, bool Include_Semicolon = true)
        {
            return INSERT(tableName, qColumnNames_Obj(obj), qValuesWithoutProcessing(obj), Include_Semicolon);
        }



        public  string INSERT_RETURN_ALL(string tableName, object[] values)
        {
            return $"{INSERT(tableName, values, false)} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN_ALL(string tableName, params KeyValuePair<string, object>[] vals)
        {
            return $"{INSERT(tableName, false, vals )} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN_ALL(string tableName, IEnumerable<object[]> values)
        {
            return $"{INSERT(tableName, values, false)} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN_ALL(string tableName, string[] columnNames, IEnumerable<object[]> values)
        {
            return $"{INSERT(tableName, columnNames, values, false)} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN_ALL(string tableName, object obj)
        {
            return $"{INSERT(tableName, obj, false)} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN_ALL(string tableName, IEnumerable<object> obj)
        {
            return $"{INSERT(tableName, obj, false)} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }


        public  string INSERT_RETURN(string tableName, string[] returnColumnNames, object[] values)
        {
            return $"{INSERT(tableName, values, false)} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN(string tableName, string[] returnColumnNames, KeyValuePair<string, object>[] vals)
        {
            return $"{INSERT(tableName, false, vals )} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN(string tableName, IEnumerable<object[]> values, string[] returnColumnNames)
        {
            return $"{INSERT(tableName, values, false)} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN(string tableName, string[] columnNames, IEnumerable<object[]> values, string[] returnColumnNames)
        {
            return $"{INSERT(tableName, columnNames, values, false)} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN(string tableName, object obj, string[] returnColumnNames)
        {
            return $"{INSERT(tableName, obj, false)} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }
        public  string INSERT_RETURN(string tableName, IEnumerable<object> obj, string[] returnColumnNames)
        {
            return $"{INSERT(tableName, obj, false)} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }



        public  string UPDATE(string tableName, bool Include_Semicolon = true, bool IsConvertable = true, params KeyValuePair<string, object>[] vals)
        {
            return $"{qUpdate(tableName)} {QueryCommands.SET} {qSet(IsConvertable, vals)} {qSemicolon(Include_Semicolon)}";
        }

        public  string UPDATE(string tableName, object obj, bool Include_Semicolon = true, bool IsConvertable = true)
        {
            return UPDATE(tableName, Include_Semicolon, IsConvertable, qColumnNames_Values_Obj(obj));
        }
        public string UPDATE(string tableName, QueryFilter filter, bool Include_Semicolon = true, bool IsConvertable = true, params KeyValuePair<string, object>[] vals)
        {
            var f = new QueryFilterHandler().ProccessFilter(filter);
            if (string.IsNullOrWhiteSpace(f)) return null;
            return $"{qUpdate(tableName)} {QueryCommands.SET} {qSet(IsConvertable, vals)} {f} {qSemicolon(Include_Semicolon)}";
        }
        public string UPDATE(string tableName, QueryFilter filter, params KeyValuePair<string, object>[] vals)
        {
            return UPDATE(tableName, filter, true, true, vals);
        }
        //public string UPDATE(string tableName, KeyValuePair<string, object> val, QueryFilter filter, bool Include_Semicolon = true)
        //{
        //    return UPDATE(tableName, new string[] { columnName }, new object[] { value }, filter, Include_Semicolon);
        //}
        public  string UPDATE(string tableName, object obj, QueryFilter filter, bool Include_Semicolon = true)
        {
            return UPDATE(tableName, filter, Include_Semicolon, false, qColumnNames_Values_Obj(obj));
        }



        public  string UPDATE_RETURN_ALL(string tableName, params KeyValuePair<string, object>[] vals)
        {
            return $"{UPDATE(tableName, true, false, vals)} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }
        //public string UPDATE_RETURN_ALL(string tableName, KeyValuePair<string, object> val)
        //{
        //    return UPDATE_RETURN_ALL(tableName, new string[] { columnName }, new object[] { value });
        //}
        public  string UPDATE_RETURN_ALL(string tableName, object obj)
        {
            return UPDATE_RETURN_ALL(tableName, qColumnNames_Values_Obj(obj));
        }
        public  string UPDATE_RETURN_ALL(string tableName, QueryFilter filter, params KeyValuePair<string, object>[] vals)
        {
            var f = new QueryFilterHandler().ProccessFilter(filter);
            if (string.IsNullOrWhiteSpace(f)) return null;
            return $"{UPDATE(tableName, filter, true, false, vals)} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }
        //public string UPDATE_RETURN_ALL(string tableName, KeyValuePair<string, object> val, QueryFilter filter)
        //{
        //    return UPDATE_RETURN_ALL(tableName, new string[] { columnName }, new object[] { value }, filter);
        //}
        public  string UPDATE_RETURN_ALL(string tableName, object obj, QueryFilter filter)
        {
            return UPDATE_RETURN_ALL(tableName, qColumnNames_Values_Obj(obj), filter);
        }

        public  string UPDATE_RETURN(string tableName, string[] returnColumnNames, params KeyValuePair<string, object>[] vals)
        {
            return $"{UPDATE(tableName, true, false, vals)} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }
        //public string UPDATE_RETURN(string tableName, KeyValuePair<string, object> val, string[] returnColumnNames)
        //{
        //    return UPDATE_RETURN(tableName, new string[] { columnName }, new object[] { value }, returnColumnNames);
        //}
        public  string UPDATE_RETURN(string tableName, object obj, string[] returnColumnNames)
        {
            return UPDATE_RETURN(tableName, qColumnNames_Values_Obj(obj), returnColumnNames);
        }
        public  string UPDATE_RETURN(string tableName, QueryFilter filter, string[] returnColumnNames, params KeyValuePair<string, object>[] vals)
        {
            var f = new QueryFilterHandler().ProccessFilter(filter);
            if (string.IsNullOrWhiteSpace(f)) return null;
            return $"{UPDATE(tableName, filter, true, false, vals)} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }
        //public string UPDATE_RETURN(string tableName, KeyValuePair<string, object> val, QueryFilter filter, string[] returnColumnNames)
        //{
        //    return UPDATE_RETURN(tableName, new string[] { columnName }, new object[] { value }, filter, returnColumnNames);
        //}
        public  string UPDATE_RETURN(string tableName, object obj, QueryFilter filter, string[] returnColumnNames)
        {
            return UPDATE_RETURN(tableName, qColumnNames_Values_Obj(obj), filter, returnColumnNames);
        }



        public  string UPSERT(string tableName, string[] conflictColumnNames, bool Include_Semicolon = true, params KeyValuePair<string, object>[] vals)
        {
            return $"{INSERT(tableName, false, vals)} {QueryCommands.ON} {QueryCommands.CONFLICT} {qBrackets(qColumnNames(conflictColumnNames))} {QueryCommands.DO} {UPDATE(tableName, true, false, vals)} {qSemicolon(Include_Semicolon)}";
        }
        public  string UPSERT(string tableName, object obj, string[] conflictColumnNames, bool Include_Semicolon = true)
        {
            return UPSERT(tableName, qColumnNames_Values_Obj(obj), conflictColumnNames, Include_Semicolon);
        }
        public  string UPSERT(string tableName, string[] conflictColumnNames, QueryFilter filter, bool Include_Semicolon = true, params KeyValuePair<string, object>[] vals)
        {
            var f = new QueryFilterHandler().ProccessFilter(filter);
            if (string.IsNullOrWhiteSpace(f)) return null;
            return $"{INSERT(tableName, false, vals)} {QueryCommands.ON} {QueryCommands.CONFLICT} {qBrackets(qColumnNames(conflictColumnNames))} {QueryCommands.DO} {UPDATE(tableName, true, false, vals)} {f} {qSemicolon(Include_Semicolon)}";
        }
        public  string UPSERT(string tableName, object obj, string[] conflictColumnNames, QueryFilter filter, bool Include_Semicolon = true)
        {
            return UPSERT(tableName, qColumnNames_Values_Obj(obj), conflictColumnNames, filter, Include_Semicolon);
        }

        public  string UPSERT_RETURN_ALL(string tableName, string[] conflictColumnNames, params KeyValuePair<string, object>[] vals)
        {
            return $"{UPSERT(tableName,conflictColumnNames, false, vals)} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }
        public  string UPSERT_RETURN_ALL(string tableName, object obj, string[] conflictColumnNames)
        {
            return UPSERT_RETURN_ALL(tableName, qColumnNames_Values_Obj(obj), conflictColumnNames);
        }
        public  string UPSERT_RETURN_ALL(string tableName, string[] conflictColumnNames, QueryFilter filter, params KeyValuePair<string, object>[]  vals)
        {
            var f = new QueryFilterHandler().ProccessFilter(filter);
            if (string.IsNullOrWhiteSpace(f)) return null;
            return $"{UPSERT(tableName, conflictColumnNames, filter, false, vals)} {QueryCommands.RETURNING} {QueryCommands.ASTERISK} {QueryCommands.SEMICOLON}";
        }
        public  string UPSERT_RETURN_ALL(string tableName, object obj, string[] conflictColumnNames, QueryFilter filter)
        {
            return UPSERT_RETURN_ALL(tableName, qColumnNames_Values_Obj(obj), conflictColumnNames, filter);
        }


        public  string UPSERT_RETURN(string tableName, string[] conflictColumnNames, string[] returnColumnNames, params KeyValuePair<string, object>[] vals)
        {
            return $"{UPSERT(tableName, conflictColumnNames, false, vals)} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }
        public  string UPSERT_RETURN(string tableName, object obj, string[] conflictColumnNames, string[] returnColumnNames)
        {
            return UPSERT_RETURN(tableName, qColumnNames_Values_Obj(obj), conflictColumnNames, returnColumnNames);
        }

        public  string UPSERT_RETURN(string tableName, string[] conflictColumnNames, QueryFilter filter, string[] returnColumnNames, params KeyValuePair<string, object>[] vals)
        {
            var f = new QueryFilterHandler().ProccessFilter(filter);
            if (string.IsNullOrWhiteSpace(f)) return null;
            return $"{UPSERT(tableName, conflictColumnNames, filter, false, vals)} {QueryCommands.RETURNING} {qColumnNames(returnColumnNames)} {QueryCommands.SEMICOLON}";
        }
        public  string UPSERT_RETURN(string tableName, object obj, string[] conflictColumnNames, QueryFilter filter, string[] returnColumnNames)
        {
            return UPSERT_RETURN(tableName, qColumnNames_Values_Obj(obj), conflictColumnNames, filter, returnColumnNames);
        }

        ////===== Where =====//
        //private  string Where(string[] WHEREcolumnNames, string[] WHEREoperators, object[] WHEREcolumnValues)
        //{
        //    //if (!(WHEREcolumnNames.Length == WHEREoperators.Length == WHEREcolumnValues.Length))
        //    //{
        //    //    throw;
        //    //}

        //    var tempArray = new string[WHEREcolumnNames.Length];

        //    for (var i = 0; i < WHEREcolumnNames.Length; i++)
        //    {
        //        WHEREoperators[i] = WHEREoperators[i].ToUpper();
        //        if (WHEREoperators[i] == "IN")
        //        {
        //            tempArray[i] = (String.Format("\"{0}\" {1} {2}", WHEREcolumnNames[i], WHEREoperators[i], PostgresConverter.ObjectValueToPostgreString_IN(WHEREcolumnValues[i])));
        //        }
        //        else
        //        {
        //            tempArray[i] = (String.Format("\"{0}\" {1} {2}", WHEREcolumnNames[i], WHEREoperators[i], PostgresConverter.ObjectValueToPostgreString(WHEREcolumnValues[i])));
        //        }
        //    }
        //    return String.Format("WHERE {0}", String.Join(" AND ", tempArray));
        //}

        ////===== Select =====//
        //public  string Select(string tableName, string[] fields, string[] WHEREcolumnNames, string[] WHEREoperators, object[] WHEREcolumnValues)
        //{
        //    string sql;
        //    //Organizing Seleceted Fileds 
        //    string tempFileds = null;
        //    if (fields != null && fields.Any())
        //    {
        //        for (var i = 0; i < fields.Length; i++)
        //        {
        //            fields[i] = "\"" + fields[i] + "\"";
        //        }
        //        tempFileds = String.Join(",", fields);
        //    }

        //    if (WHEREcolumnValues == null || WHEREcolumnNames == null || WHEREoperators == null)
        //    {
        //        sql = String.Format("SELECT {1} FROM \"{0}\";", tableName, tempFileds ?? "*");
        //    }
        //    else
        //    {
        //        sql = String.Format("SELECT {2} FROM \"{0}\" {1};", tableName, Where(WHEREcolumnNames, WHEREoperators, WHEREcolumnValues), tempFileds ?? "*");
        //    }

        //    return sql;
        //}

        //public  string Select(string tableName, string[] WHEREcolumnNames, string[] WHEREoperators, object[] WHEREcolumnValues)
        //{
        //    return Select(tableName, null, WHEREcolumnNames, WHEREoperators, WHEREcolumnValues);
        //}

        //public  string Select(string tableName)
        //{
        //    return String.Format("SELECT * FROM \"{0}\";", tableName);
        //}

        //public  string Select(string tableName, object obj)
        //{
        //    var properties = obj.GetType().GetProperties();
        //    var pk_columnNames = EntityHandler.GetPrimaryKeysFromEntity(obj);

        //    var columnIndex = pk_columnNames.Count();

        //    var WHEREcolumnNames = new string[columnIndex];
        //    var WHEREoperators = new string[columnIndex];
        //    var WHEREcolumnValues = new object[columnIndex];

        //    if (pk_columnNames == null || !pk_columnNames.Any())
        //    {
        //        columnIndex = properties.Count();
        //        WHEREcolumnNames = new string[columnIndex];
        //        WHEREoperators = new string[columnIndex];
        //        WHEREcolumnValues = new object[columnIndex];
        //    }


        //    for (var i = 0; i < columnIndex; i++)
        //    {
        //        WHEREcolumnNames[i] = properties[i].Name;
        //        WHEREoperators[i] = "=";
        //        WHEREcolumnValues[i] = properties[i].GetValue(obj, null);

        //    }
        //    return Select(tableName, null, WHEREcolumnNames, WHEREoperators, WHEREcolumnValues);
        //}

        ////===== Select WHERE NOT EXISTS=====//
        //public  string Select_1_WHERE_NOT_EXISTS(string tableName, object[] values, string[] WHEREcolumnNames, string[] WHEREoperators, object[] WHEREcolumnValues)
        //{
        //    string sql;
        //    string tempValues = null;
        //    if (values != null && values.Any())
        //    {
        //        var tempValArr = new string[values.Length];
        //        for (var i = 0; i < values.Length; i++)
        //        {
        //            tempValArr[i] = PostgresConverter.ObjectValueToPostgreString(values[i]);
        //        }
        //        tempValues = String.Join(",", tempValArr);
        //    }

        //    if (WHEREcolumnValues == null || WHEREcolumnNames == null || WHEREoperators == null)
        //    {
        //        return String.Format("SELECT {0} WHERE NOT EXISTS (SELECT 0);", tempValues);
        //    }
        //    else
        //    {
        //        var tempArray = new string[WHEREcolumnNames.Length];

        //        for (var i = 0; i < WHEREcolumnNames.Length; i++)
        //        {
        //            if (WHEREoperators[i].ToUpper() == "IN")
        //            {
        //                tempArray[i] = (String.Format("\"{0}\" {1} {2}", WHEREcolumnNames[i], WHEREoperators[i], PostgresConverter.ObjectValueToPostgreString_IN(WHEREcolumnValues[i])));
        //            }
        //            else
        //            {
        //                tempArray[i] = (String.Format("\"{0}\" {1} {2}", WHEREcolumnNames[i], WHEREoperators[i], PostgresConverter.ObjectValueToPostgreString(WHEREcolumnValues[i])));
        //            }
        //        }
        //        sql = String.Format("SELECT {2} WHERE NOT EXISTS (SELECT 1 FROM \"{0}\" WHERE {1});", tableName, String.Join(" AND ", tempArray), tempValues);
        //    }

        //    return sql;
        //}

        ////===== Delete =====//
        //public  string Delete(string tableName)
        //{
        //    return String.Format("DELETE FROM \"{0}\";", tableName);
        //}

        //public  string Delete(string tableName, string[] WHEREcolumnNames, string[] WHEREoperators, object[] WHEREcolumnValues)
        //{
        //    if (WHEREcolumnValues == null || WHEREcolumnNames == null || WHEREoperators == null)
        //    {
        //        return Delete(tableName);
        //    }
        //    else
        //    {
        //        return String.Format("DELETE FROM \"{0}\" {1};", tableName, Where(WHEREcolumnNames, WHEREoperators, WHEREcolumnValues));
        //    }
        //}
        //public  string Delete(DataTable dt)
        //{
        //    var tempDT = dt.GetChanges(DataRowState.Deleted);
        //    if (tempDT == null)
        //        return null;

        //    var pk_columnNames = dt.PrimaryKey.Select(q => q.ColumnName).ToArray();
        //    if (!pk_columnNames.Any())
        //        return null;

        //    var columnIndex = pk_columnNames.Count();
        //    var WHEREcolumnNames = pk_columnNames;
        //    var WHEREoperators = new string[columnIndex];
        //    for (var i = 0; i < columnIndex; i++)
        //    {
        //        WHEREoperators[i] = "=";
        //    }
        //    var tempSQL = new List<string>();
        //    foreach (DataRow row in tempDT.Rows)
        //    {
        //        var WHEREcolumnValues = new object[columnIndex];
        //        for (var i = 0; i < columnIndex; i++)
        //        {
        //            WHEREcolumnValues[i] = row[pk_columnNames[i], DataRowVersion.Original];
        //        }
        //        tempSQL.Add(Delete(dt.TableName, WHEREcolumnNames, WHEREoperators, WHEREcolumnValues));

        //    }
        //    return String.Join(";", tempSQL) + ";";
        //}
        //public  string Delete(DataRow dr)
        //{
        //    if (dr.RowState == DataRowState.Deleted)
        //        return null;

        //    var pk_columnNames = dr.Table.PrimaryKey.Select(q => q.ColumnName).ToArray();
        //    if (!pk_columnNames.Any())
        //        return null;

        //    var columnIndex = pk_columnNames.Count();
        //    var WHEREcolumnNames = pk_columnNames;
        //    var WHEREoperators = new string[columnIndex];
        //    for (var i = 0; i < columnIndex; i++)
        //    {
        //        WHEREoperators[i] = "=";
        //    }
        //    var WHEREcolumnValues = new object[columnIndex];
        //    for (var i = 0; i < columnIndex; i++)
        //    {
        //        WHEREcolumnValues[i] = dr[pk_columnNames[i], DataRowVersion.Original];
        //    }
        //    return Delete(dr.Table.TableName, WHEREcolumnNames, WHEREoperators, WHEREcolumnValues);
        //}
        //public  string Delete(string tableName, object obj)
        //{

        //    var properties = obj.GetType().GetProperties();
        //    var pk_columnNames = EntityHandler.GetPrimaryKeysFromEntity(obj);

        //    var columnIndex = pk_columnNames.Count();

        //    var WHEREcolumnNames = new string[columnIndex];
        //    var WHEREoperators = new string[columnIndex];
        //    var WHEREcolumnValues = new object[columnIndex];

        //    if (pk_columnNames == null || !pk_columnNames.Any())
        //    {
        //        columnIndex = properties.Count();
        //        WHEREcolumnNames = new string[columnIndex];
        //        WHEREoperators = new string[columnIndex];
        //        WHEREcolumnValues = new object[columnIndex];
        //    }

        //    for (var i = 0; i < columnIndex; i++)
        //    {
        //        WHEREcolumnNames[i] = properties[i].Name;
        //        WHEREoperators[i] = "=";
        //        WHEREcolumnValues[i] = properties[i].GetValue(obj, null);

        //    }

        //    return Delete(tableName, WHEREcolumnNames, WHEREoperators, WHEREcolumnValues);
        //}

        ////===== Insert =====//
        //public  string Insert(string tableName, object[] values)
        //{
        //    var tempArray = new string[values.Length];
        //    for (var i = 0; i < values.Length; i++)
        //    {
        //        tempArray[i] = PostgresConverter.ObjectValueToPostgreString(values[i]);
        //    }
        //    return String.Format("INSERT INTO \"{0}\" VALUES({1});", tableName, String.Join(", ", tempArray));
        //}
        //public  string Insert(string tableName, KeyValuePair<string, object>[] vals)
        //{
        //    var tempArray = new string[values.Length];
        //    for (var i = 0; i < values.Length; i++)
        //    {
        //        columnNames[i] = "\"" + columnNames[i] + "\"";
        //        tempArray[i] = PostgresConverter.ObjectValueToPostgreString(values[i]);
        //    }
        //    return String.Format("INSERT INTO \"{0}\" ({2})  VALUES({1});", tableName, String.Join(", ", tempArray), String.Join(", ", columnNames));
        //}
        //public  string Insert(string tableName, List<object[]> values)
        //{
        //    var tempSQL = new List<string>();
        //    foreach (var val in values)
        //    {
        //        var tempArray = new string[val.Length];
        //        for (var i = 0; i < val.Length; i++)
        //        {
        //            tempArray[i] = PostgresConverter.ObjectValueToPostgreString(val[i]);
        //        }
        //        tempSQL.Add(String.Format("({0})", String.Join(", ", tempArray)));
        //    }

        //    return String.Format("INSERT INTO \"{0}\" VALUES {1};", tableName, String.Join(", ", tempSQL));
        //}
        //public  string Insert(string tableName, string[] columnNames, List<object[]> values)
        //{
        //    var tempSQL = values.Select(val => Insert(tableName, columnNames, val)).ToList();
        //    return String.Join("; ", tempSQL) + ";";
        //}
        //public  string Insert(DataTable dt)
        //{
        //    var tempDT = dt.GetChanges(DataRowState.Added);
        //    if (tempDT == null)
        //        return null;
        //    var columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
        //    var columnNamesSQL = dt.Columns.Cast<DataColumn>().Select(x => "\"" + x.ColumnName + "\"").ToArray();
        //    var tempSQL = new List<string>();
        //    foreach (DataRow row in tempDT.Rows)
        //    {
        //        var tempArray = new string[columnNames.Length];

        //        for (var i = 0; i < columnNames.Length; i++)
        //        {
        //            tempArray[i] = PostgresConverter.ObjectValueToPostgreString(row[columnNames[i]]);
        //        }

        //        tempSQL.Add(String.Format("({0})", String.Join(", ", tempArray)));
        //    }

        //    return String.Format("INSERT INTO \"{0}\" ({1}) VALUES {2};", dt.TableName, String.Join(", ", columnNamesSQL), String.Join(", ", tempSQL));
        //}
        //public  string Insert(DataRow dr)
        //{
        //    if (dr.RowState != DataRowState.Added)
        //        return null;
        //    var columnNames = dr.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
        //    var columnNamesSQL = dr.Table.Columns.Cast<DataColumn>().Select(x => "\"" + x.ColumnName + "\"").ToArray();
        //    var tempSQL = new List<string>();
        //    var tempArray = new string[columnNames.Length];

        //    for (var i = 0; i < columnNames.Length; i++)
        //    {
        //        tempArray[i] = PostgresConverter.ObjectValueToPostgreString(dr[columnNames[i]]);
        //    }

        //    tempSQL.Add(String.Format("({0})", String.Join(", ", tempArray)));

        //    return String.Format("INSERT INTO \"{0}\" ({1}) VALUES {2};", dr.Table.TableName, String.Join(", ", columnNamesSQL), String.Join(", ", tempSQL));
        //}
        //public  string Insert(string tableName, object obj)
        //{
        //    var tempArray = new List<string>();
        //    var columnNamesSQL = new List<string>();
        //    var properties = obj.GetType().GetProperties();
        //    var f_jsonArrayNames = EntityHandler.GetJsonArrayFiledEntity(obj);
        //    var insertqueryValueProperties = EntityHandler.GetQueryValueAttributeFromEntity(obj);

        //    foreach (var objProperty in properties)
        //    {
        //        if (IsValidColumnName(objProperty.Name))
        //        {

        //            if (f_jsonArrayNames.IsNullOrEmptyContains(objProperty.Name))
        //            {
        //                columnNamesSQL.Add("\"" + objProperty.Name + "\"");
        //                tempArray.Add(PostgresConverter.ObjectValueToPostgreString(PostgresConverter.ConvertJsonArrayToPostgesJsonArray(objProperty.GetValue(obj, null))));
        //            }
        //            else if (insertqueryValueProperties.IsNullOrEmptyContains(objProperty.Name))
        //            {
        //                columnNamesSQL.Add("\"" + objProperty.Name + "\"");
        //                var temp_qeury_val = EntityHandler.GetQueryValueAttributeValue(objProperty);
        //                tempArray.Add(temp_qeury_val);
        //            }
        //            else
        //            {
        //                columnNamesSQL.Add("\"" + objProperty.Name + "\"");

        //                tempArray.Add(PostgresConverter.ObjectValueToPostgreString(objProperty.GetValue(obj, null)));
        //            }
        //        }

        //    }
        //    return String.Format("INSERT INTO \"{0}\" ({1}) VALUES {2};", tableName, String.Join(", ", columnNamesSQL), String.Format("({0})", String.Join(", ", tempArray)));
        //}


        ////===== Insert_Return =====//
        //public  string Insert_Return(string tableName, object[] values, string[] returnColumnNames)
        //{
        //    var query = Insert(tableName, values);
        //    if (returnColumnNames != null && returnColumnNames.Count() != 0)
        //    {
        //        for (var i = 0; i < returnColumnNames.Length; i++)
        //        {
        //            returnColumnNames[i] = "\"" + returnColumnNames[i] + "\"";
        //        }
        //        return String.Format("{0} RETURNING {1};", query, String.Join(", ", returnColumnNames));
        //    }
        //    else
        //    {
        //        return String.Format("{0} RETURNING * ;", query);
        //    }
        //}
        //public  string Insert_Return(string tableName, List<object[]> values, string[] returnColumnNames)
        //{
        //    var query = Insert(tableName, values);
        //    if (returnColumnNames != null && returnColumnNames.Count() != 0)
        //    {
        //        for (var i = 0; i < returnColumnNames.Length; i++)
        //        {
        //            returnColumnNames[i] = "\"" + returnColumnNames[i] + "\"";
        //        }
        //        return String.Format("{0} RETURNING {1} ;", query, String.Join(", ", returnColumnNames));
        //    }
        //    else
        //    {
        //        return String.Format("{0} RETURNING * ;", query);
        //    }
        //}


        ////===== Update =====//
        //public  string Update(string tableName, KeyValuePair<string, object>[] vals, string[] WHEREcolumnNames, string[] WHEREoperators, object[] WHEREcolumnValues)
        //{
        //    var tempValues = new string[values.Length];
        //    for (var i = 0; i < values.Length; i++)
        //    {
        //        columnNames[i] = "\"" + columnNames[i] + "\"";
        //        tempValues[i] = PostgresConverter.ObjectValueToPostgreString(values[i]);
        //    }
        //    return String.Format("UPDATE \"{0}\" SET ({1}) = ({2}) {3} ;", tableName, String.Join(", ", columnNames), String.Join(", ", tempValues), Where(WHEREcolumnNames, WHEREoperators, WHEREcolumnValues));
        //}

        //public  string Update(DataTable dt)
        //{
        //    var tempDT = dt.GetChanges(DataRowState.Modified);
        //    if (tempDT == null)
        //        return null;
        //    var pk_columnNames = dt.PrimaryKey.Select(q => q.ColumnName).ToArray();
        //    if (!pk_columnNames.Any())
        //        return null;
        //    var columnNames = dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Except(pk_columnNames).ToArray();
        //    var columnNamesSQL = columnNames.Select(x => "\"" + x + "\"").ToArray();

        //    var tempSQL = new List<string>();
        //    foreach (DataRow row in tempDT.Rows)
        //    {
        //        //Values
        //        var tempArray = new string[columnNames.Length];
        //        for (var i = 0; i < columnNames.Length; i++)
        //        {
        //            tempArray[i] = PostgresConverter.ObjectValueToPostgreString(row[columnNames[i]]);
        //        }

        //        //Where
        //        var pkArray = new string[pk_columnNames.Length];
        //        for (var i = 0; i < pk_columnNames.Length; i++)
        //        {
        //            pkArray[i] = (String.Format("\"{0}\" = {1}", pk_columnNames[i], PostgresConverter.ObjectValueToPostgreString(row[pk_columnNames[i]])));
        //        }

        //        tempSQL.Add(String.Format("UPDATE \"{0}\" SET ({1}) = ({2}) WHERE {3}", dt.TableName, String.Join(", ", columnNamesSQL), String.Join(", ", tempArray), String.Join(" AND ", pkArray)));
        //    }

        //    return String.Join(";", tempSQL) + ";";
        //}

        //public  string Update(DataRow dr)
        //{
        //    if (dr.RowState != DataRowState.Modified)
        //        return null;
        //    var pk_columnNames = dr.Table.PrimaryKey.Select(q => q.ColumnName).ToArray();
        //    if (!pk_columnNames.Any())
        //        return null;
        //    var columnNames = dr.Table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).Except(pk_columnNames).ToArray();
        //    var columnNamesSQL = columnNames.Select(x => "\"" + x + "\"").ToArray();


        //    //Values
        //    var tempArray = new string[columnNames.Length];
        //    for (var i = 0; i < columnNames.Length; i++)
        //    {
        //        tempArray[i] = PostgresConverter.ObjectValueToPostgreString(dr[columnNames[i]]);
        //    }

        //    //Where
        //    var pkArray = new string[pk_columnNames.Length];
        //    for (var i = 0; i < pk_columnNames.Length; i++)
        //    {
        //        pkArray[i] = (String.Format("\"{0}\" = {1}", pk_columnNames[i], PostgresConverter.ObjectValueToPostgreString(dr[pk_columnNames[i]])));
        //    }

        //    return String.Format("UPDATE \"{0}\" SET ({1}) = ({2}) WHERE {3}", dr.Table.TableName, String.Join(", ", columnNamesSQL), String.Join(", ", tempArray), String.Join(" AND ", pkArray));
        //}

        //public  string Update(string tableName, object obj)
        //{
        //    var update_obj = obj.GetType();
        //    var dirtyProperties = (List<string>)obj.GetType().GetProperty("e_DirtyProperties").GetValue(obj, null);
        //    if (!dirtyProperties.IsNullOrEmpty())
        //    {
        //        var columnNames = new List<string>();
        //        var tempValues = new List<object>();
        //        var pk_columnNames = EntityHandler.GetPrimaryKeysFromEntity(obj);
        //        var f_jsonArrayNames = EntityHandler.GetJsonArrayFiledEntity(obj);
        //        var pk_columnIndex = pk_columnNames.Count();

        //        for (var i = 0; i < dirtyProperties.Count; i++)
        //        {

        //            if (!pk_columnNames.Contains(dirtyProperties[i]) && IsValidColumnName(dirtyProperties[i]))
        //            {
        //                if (f_jsonArrayNames != null && f_jsonArrayNames.Contains(dirtyProperties[i]))
        //                {
        //                    columnNames.Add(dirtyProperties[i]);
        //                    tempValues.Add(PostgresConverter.ConvertJsonArrayToPostgesJsonArray(update_obj.GetProperty(dirtyProperties[i]).GetValue(obj, null)));
        //                }
        //                else
        //                {
        //                    columnNames.Add(dirtyProperties[i]);
        //                    tempValues.Add(update_obj.GetProperty(dirtyProperties[i]).GetValue(obj, null));
        //                }

        //            }
        //        }

        //        var WHEREcolumnNames = new string[pk_columnIndex];
        //        var WHEREoperators = new string[pk_columnIndex];
        //        var WHEREcolumnValues = new object[pk_columnIndex];
        //        for (var i = 0; i < pk_columnIndex; i++)
        //        {
        //            WHEREcolumnNames[i] = pk_columnNames[i];
        //            WHEREoperators[i] = "=";
        //            var propertyInfo = obj.GetType().GetProperty(pk_columnNames[i]);
        //            WHEREcolumnValues[i] = propertyInfo.GetValue(obj, null);


        //        }
        //        return Update(tableName, columnNames.ToArray(), tempValues.ToArray(), WHEREcolumnNames, WHEREoperators, WHEREcolumnValues);
        //    }

        //    return null;
        //}

        ////===== Update_Return =====//
        //public  string Update_Return(string tableName, KeyValuePair<string, object>[] vals, string[] WHEREcolumnNames, string[] WHEREoperators, object[] WHEREcolumnValues, string[] returnColumnNames)
        //{
        //    var query = Update(tableName, columnNames, values, WHEREcolumnNames, WHEREoperators, WHEREcolumnValues);
        //    if (returnColumnNames != null && returnColumnNames.Count() != 0)
        //    {
        //        for (var i = 0; i < returnColumnNames.Length; i++)
        //        {
        //            returnColumnNames[i] = "\"" + returnColumnNames[i] + "\"";
        //        }
        //        return String.Format("{0} RETURNING {1} ;", query, String.Join(", ", returnColumnNames));
        //    }
        //    else
        //    {
        //        return String.Format("{0} RETURNING * ;", query);
        //    }
        //}

        ////===== Update id not Exist Insert =====//
        //public  string Update_ifNotExist_Insert(string tableName, KeyValuePair<string, object>[] vals, string[] WHEREcolumnNames, string[] WHEREoperators, object[] WHEREcolumnValues)
        //{
        //    var sql1 = Update(tableName, columnNames, values, WHEREcolumnNames, WHEREoperators, WHEREcolumnValues);
        //    var sql2 = String.Format("INSERT INTO \"{0}\" ({1}) {2} ", tableName, String.Join(", ", columnNames), Select_1_WHERE_NOT_EXISTS(tableName, values, WHEREcolumnNames, WHEREoperators, WHEREcolumnValues));
        //    return sql1 + sql2;
        //}
    }
}
