using UnivIntel.PostgreSQL.ORM.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnivIntel.PostgreSQL.ORM.Core.Helpers
{
    public class EntityAttributeHps
    {

        //public string GetTableName<T>() where T : new()
        //{
        //   var = obj new T();
        //   var r =  typeof(T).GetCustomAttributes(typeof(PgsDataTable), false).First() as PgsDataTable
        //}


        //public IEnumerable<string> GetPropertyNamesWhereAttributeExist<T>()
        //{
        //    var tempList = new List<string>();
        //    var properties = this.GetType().GetProperties();
        //    foreach (var objProperty in properties)
        //    {
        //        var attrs = objProperty.GetCustomAttributes(true);
        //        foreach (var attr in attrs)
        //        {

        //            try
        //            {
        //                var tempAttr = (T)attr;
        //                if (tempAttr != null)
        //                {
        //                    tempList.Add(objProperty.Name);
        //                }
        //            }
        //            catch
        //            {

        //            }

        //        }
        //    }
        //    return tempList.Any() ? tempList : null;
        //}

        //public IEnumerable<string> FirstOrDefaultGetPKName(object obj)
        //{

        //    if()
        //    return GetPropertyNamesWhereAttributeExist<PgsPK>(obj);
        //}

        //public  IEnumerable<string> GetJsonArrayFiledEntity(object obj)
        //{
        //    return GetPropertyNamesWhereAttributeExist<JsonArrayPostgreSQL>(obj);
        //}
    }
}
