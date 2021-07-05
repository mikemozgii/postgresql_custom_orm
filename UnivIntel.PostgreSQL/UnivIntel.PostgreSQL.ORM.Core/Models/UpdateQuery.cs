using UnivIntel.PostgreSQL.Core;
using UnivIntel.PostgreSQL.ORM.Core.Handlers;
using UnivIntel.PostgreSQL.ORM.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Models
{
    public class UpdateQuery<T> : BaseQueryGeneric<T> where T : class
    {
        private ReturnDataSettings ReturnDataSettings { get; set; }
        private object Value { get; set; }
        private KeyValuePair<string, object>[] Values {get;set;}
        private QueryFilter Filter { get; set; }

        public UpdateQuery(object _value, QueryFilter _filter, ReturnDataSettings _ReturnDataSettings = null) 
        {
            ReturnDataSettings = _ReturnDataSettings;
            Filter = _filter;
            ExpectResults = ReturnDataSettings != null;
            Value =  _value;
            Init();
        }

        public UpdateQuery(QueryFilter _filter, ReturnDataSettings _ReturnDataSettings = null, params KeyValuePair<string, object>[] vals) 
        {
            ReturnDataSettings = _ReturnDataSettings;
            Filter = _filter;
            ExpectResults = ReturnDataSettings != null;
            Values = vals;
            Init();
        }
        public UpdateQuery(QueryFilter _filter,  params KeyValuePair<string, object>[] vals)
        {
            ReturnDataSettings = null;
            Filter = _filter;
            ExpectResults = ReturnDataSettings != null;
            Values = vals;
            Init();
        }

        public void Init()
        {
            //IsValid = IsValid_InsertRequest();

            if (!Values.IsNullOrEmpty())
            {
                if (ExpectResults)
                {
                    if (ReturnDataSettings.ColumnNames.IsNullOrEmpty())
                    {
                        Request = new QueryRequest(new QueryBuilderHandler().UPDATE_RETURN_ALL(GetTableNameFromAttribute(), Filter, Values));
                    }
                    else
                    {
                        Request = new QueryRequest(new QueryBuilderHandler().UPDATE_RETURN(GetTableNameFromAttribute(), Filter, ReturnDataSettings.ColumnNames, Values));
                    }
                }
                else
                {
                    Request = new QueryRequest(new QueryBuilderHandler().UPDATE(GetTableNameFromAttribute(), Filter, true, true, Values));
                }

            }
            else
            {

                if (ExpectResults)
                {
                    if (ReturnDataSettings.ColumnNames.IsNullOrEmpty())
                    {
                        Request = new QueryRequest(new QueryBuilderHandler().UPDATE_RETURN_ALL(GetTableNameFromAttribute(), Value, Filter));
                    }
                    else
                    {
                        Request = new QueryRequest(new QueryBuilderHandler().UPDATE_RETURN(GetTableNameFromAttribute(), Value, Filter, ReturnDataSettings.ColumnNames));
                    }
                }
                else
                {
                    Request = new QueryRequest(new QueryBuilderHandler().UPDATE(GetTableNameFromAttribute(), Value, Filter));
                }
            }



        }

        //public bool IsValid_InsertRequest()
        //{
        //    if (Value == null) return false;
        //    if (ReturnValues && !ReturnAll && !ReturnColumnNames.IsNullOrEmpty()) return false;

        //    return true;
        //}


    }
}
