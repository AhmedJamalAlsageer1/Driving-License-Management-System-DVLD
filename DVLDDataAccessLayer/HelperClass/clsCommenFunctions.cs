using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLDDataAccessLayer
{
    internal class clsCommenFunctions
    {
        public static int? FromSQLToInt(SqlDataReader reader, string CurrentCell)
        {
            int Index = reader.GetOrdinal(CurrentCell);
            return !reader.IsDBNull(Index)? reader.GetInt32(Index) : (int?) null;
        }

        public static string FromSQLToString(SqlDataReader reader, string CurrentCell)
        {
            int Index = reader.GetOrdinal(CurrentCell);
            return !reader.IsDBNull(Index) ? reader.GetString(Index) : null;
        }

        public static bool? FromSQLToBOOL(SqlDataReader reader, string CurrentCell)
        {
            int Index = reader.GetOrdinal(CurrentCell);
            return !reader.IsDBNull(Index)? reader.GetBoolean(Index) : (bool?)null;
        }

        public static decimal? FromSQLToDecimal(SqlDataReader reader, string CurrentCell)
        {
            int index = reader.GetOrdinal(CurrentCell);
            return !reader.IsDBNull(index)? reader.GetDecimal(index) : (decimal?)null;
        }

        public static DateTime? FromSQLToDateTime (SqlDataReader reader, string CurrentCell)
        {
            int index = reader.GetOrdinal(CurrentCell);
            return !reader.IsDBNull(index)? reader.GetDateTime(index) : (DateTime?)null;
        }

        public static byte? FromSQLToByte(SqlDataReader reader, string CurrentCell)
        {
            int index = reader.GetOrdinal(CurrentCell);
            return !reader.IsDBNull(index)? reader.GetByte(index) : (byte?)null;
        }

        public static int? ToNullableINT(object result)
        {
            if(result == null || result == DBNull.Value)
                return null;

            if(int.TryParse(result.ToString(), out int value))
                return value;

            return null;
        }

        public static object ToDBNullIfEmpty(string Value)
        {
            return string.IsNullOrWhiteSpace(Value) ? (object) DBNull.Value : Value;
        }
    }
}
