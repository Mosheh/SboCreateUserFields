using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateUserFields.Common
{
    public static class Extentions
    {
        public static Recordset ExecuteRS(this Company company, string sql)
        {
            var recordSet = company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;

            recordSet.DoQuery(sql);

            return recordSet;
        }

        public static string TSqlToAnsi(this string sql)
        {
            return sql.Replace("[", "\"").Replace("]", "\"");
        }

        public static T To<T>(this object value)
        {
            Type t = typeof(T);

            // Get the type that was made nullable.
            Type valueType = Nullable.GetUnderlyingType(typeof(T));

            if (valueType != null)
            {
                // Nullable type.

                if (value == null)
                {
                    // you may want to do something different here.
                    return default(T);
                }
                else
                {
                    // Convert to the value type.
                    object result = Convert.ChangeType(value, valueType);

                    // Cast the value type to the nullable type.
                    return (T)result;
                }
            }
            else
            {
                // Not nullable.
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
    }

    
}
