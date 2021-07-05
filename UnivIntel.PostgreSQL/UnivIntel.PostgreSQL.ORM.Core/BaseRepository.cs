using AutoMapper;
using UnivIntel.PostgreSQL.Core;
using UnivIntel.PostgreSQL.ORM.Core.Attributes;
using UnivIntel.PostgreSQL.ORM.Core.Enums;
using UnivIntel.PostgreSQL.ORM.Core.Models;
using UnivIntel.PostgreSQL.ORM.Core.Services;
using UnivIntel.PostgreSQL.ORM.Core.Uuid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UnivIntel.PostgreSQL.ORM.Core
{
    public class BaseRepository<T> where T : class
    {
        [PgsNotMapped]
        [IgnoreMap]
        private string EntityPrimaryKeyName { get; set; }
        [PgsNotMapped]
        [IgnoreMap]
        private object EntityPrimaryKeyValue { get; set; }
        public BaseRepository()
        {
            FillEntityPrimaryKey();
        }

        public QueryRequest SELECTALL()
        {
            if (EntityPrimaryKeyValue.GetType().Name.Contains("Int32"))
            {
                return new SelectQueryGeneric<T>(new QueryFilter(new QueryFilter(EntityPrimaryKeyName, QueryOperators.NOTEQ, -1))).Request;
            }
            else
            {
                return new SelectQueryGeneric<T>(new QueryFilter(new QueryFilter(EntityPrimaryKeyName, QueryOperators.NOTEQ, new Guid()))).Request;
            }
        }

        public QueryRequest SELECTbyColumnValue(params KeyValuePair<string, object>[] _val)
        {
            return new SelectQueryGeneric<T>(BuildFilterByValue(_val)).Request;
        }

        public QueryRequest SELECTbyId(object id = null)
        {
            return new SelectQueryGeneric<T>(BuildFilterByid(id)).Request;
        }
      
        public QueryRequest SELECT(QueryFilter filter)
        {
            return new SelectQueryGeneric<T>(filter).Request;
        }

        public QueryRequest INSERT()
        {
            SetEntityPrimaryKey();
            return new InsertQuery<T>(_value: this, new ReturnDataSettings(EntityPrimaryKeyName)).Request;
        }

        public QueryRequest UPDATE(QueryFilter queryFilter = null)
        {
            return (new UpdateQuery<T>(this, queryFilter ?? BuildFilterByid())).Request;
        }

        public QueryRequest UPDATE(QueryFilter queryFilter = null, params KeyValuePair<string, object>[] _values)
        {
            return new UpdateQuery<T>(queryFilter ?? BuildFilterByid(), null, _values).Request;
        }

        public QueryRequest UPDATE(object id = null, params KeyValuePair<string, object>[] _values)
        {
            return new UpdateQuery<T>(BuildFilterByid(id), _values).Request;
        }

        public QueryRequest DELETE(object id = null)
        {
            return new DeleteQuery<T>(BuildFilterByid(id)).Request;
        }

        public QueryRequest DELETE(QueryFilter queryFilter)
        {
            return new DeleteQuery<T>(queryFilter).Request;
        }

        private QueryFilter BuildFilterByid(object id = null)
        {
            FillEntityPrimaryKey();
            return new QueryFilter(new QueryFilter(EntityPrimaryKeyName, QueryOperators.EQ, id ?? EntityPrimaryKeyValue));

        }

        public void FillEntityPrimaryKey()
        {

            var propertyPK = typeof(T).GetProperties().FirstOrDefault(q => q.GetCustomAttributes(typeof(PgsPK), true) != null);
            EntityPrimaryKeyName = propertyPK.Name;
            EntityPrimaryKeyValue = propertyPK.GetValue(this, null);
        }

        private void SetEntityPrimaryKey()
        {

            var propertyPK = typeof(T).GetProperties().FirstOrDefault(q => q.GetCustomAttributes(typeof(PgsPK), true) != null);
            var pk = new PKuuidSvc().GenereateTradingGuid();
            propertyPK.SetValue(this, pk,null);
        }

        //private QueryFilter BuildFilterByValue(string column, object value)
        //{
        //    return new QueryFilter(new QueryFilter(column, QueryOperators.EQ, value));

        //}

        private QueryFilter BuildFilterByValue(params KeyValuePair<string, object>[] _val)
        {
            return new QueryFilter(_val.Select(q => new QueryFilter(q.Key, QueryOperators.EQ, q.Value)).ToArray());

        }

        //private QueryFilter BuildFilterByValue(KeyValuePair<string, object> _val)
        //{
        //    return new QueryFilter(new QueryFilter(_val.Key, QueryOperators.EQ, _val.Value));
        //}

    }
}
