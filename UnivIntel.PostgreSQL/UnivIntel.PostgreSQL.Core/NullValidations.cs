using System;
using System.Collections.Generic;
using System.Linq;

namespace UnivIntel.PostgreSQL.Core
{
    public static class NullValidations
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                return true;
            }

            var collection = enumerable as ICollection<T>;

            if (collection != null)
            {
                return collection.Count < 1;
            }

            return !enumerable.Any();
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null)
                return true;

            return collection.Count < 1;
        }

        public static List<T> IsNullOrEmptyReturnList<T>(this List<T> collection)
        {
            return collection.IsNullOrEmpty() ? null : collection;
        }

        public static List<T> IsNullOrEmptyReturn<T>(this ICollection<T> collection)
        {
            return collection.IsNullOrEmpty() ? null : collection.ToList();
        }

        public static bool IsNullOrEmptyContains<T>(this ICollection<T> collection, T obj)
        {
            if (collection == null)
                return false;

            if (collection.Count < 1)
                return false;

            if (collection.Contains(obj))
                return true;

            return false;
        }

        public static List<T> ClearNullsReturnList<T>(this ICollection<T> collection)
        {
            if (collection.IsNullOrEmpty())
                return new List<T>();

            var t = collection.Where(q => q != null).ToList();
            if (!t.IsNullOrEmpty())
                return t;

            return new List<T>();
        }

        public static List<T> ClearNullsReturnList<T>(this IEnumerable<T> enumerable)
        {
            var collection = enumerable as ICollection<T>;
            if (collection.IsNullOrEmpty())
                return new List<T>();

            var t = collection.Where(q => q != null).ToList();
            if (!t.IsNullOrEmpty())
                return t;

            return new List<T>();
        }

        public static List<T> ClearNulls<T>(this ICollection<T> collection)
        {
            if (collection.IsNullOrEmpty())
                return null;

            var t = collection.Where(q => q != null).ToList();
            if (!t.IsNullOrEmpty())
                return t;

            return null;
        }

        public static List<T> ClearNulls<T>(this IEnumerable<T> enumerable)
        {
            var collection = enumerable as ICollection<T>;
            if (collection.IsNullOrEmpty())
                return null;

            var t = collection.Where(q => q != null).ToList();
            if (!t.IsNullOrEmpty())
                return t;

            return null;
        }

        //public static bool IsNullOrEmpty(this object obj)
        //{
        //    if (obj == null) return true;

        //    var collection = obj as IEnumerable<object>;
        //    return !collection.With(x => x.Any(), collection == null);
        //}

        public static bool IsNullOrEmpty(this object obj, out int count)
        {
            count = 0;
            if (obj == null) return true;

            var collection = obj as IEnumerable<object>;
            if (collection != null)
            {
                count = collection.Count();
                return count < 1;
            }

            return false;
        }

    }
}
