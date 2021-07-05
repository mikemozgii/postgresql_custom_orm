using System;
using System.Collections.Generic;
using System.Text;

namespace UnivIntel.PostgreSQL.Core
{
 
        public static class Pairing
        {
            public static KeyValuePair<string, object> Of(string key, object value)
            {
                return new KeyValuePair<string, object>(key, value);
            }

            public static KeyValuePair<string, object> Pair(this string key, object value)
            {
                return new KeyValuePair<string, object>(key, value);
            }
        }

     
    
}
