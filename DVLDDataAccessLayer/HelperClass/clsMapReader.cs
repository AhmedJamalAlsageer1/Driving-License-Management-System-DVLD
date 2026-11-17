using SharedLayer.enums;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccessLayer
{
    internal class clsMapReader
    {
        public static bool TryMapUserID(int? UserID, ref int dtoUserID)
        {
            if (!UserID.HasValue)
                return false;
            else
            {
                dtoUserID = UserID.Value;
                return true;
            }
        }

        public static bool TryGetIntFromReaderImmediately(SqlDataReader reader, string CurrentCell, out int dtoInt)
        {
            dtoInt = -1;
            int? UserID = clsCommenFunctions.FromSQLToInt(reader, CurrentCell);
            if(TryMapUserID(UserID, ref dtoInt))
            {
                return true;
            }
            return false;
        }

        public static bool TryGetStringFromReaderImediately(SqlDataReader reader, string Currentcell, out string dtoString)
        {
            dtoString = clsCommenFunctions.FromSQLToString(reader, Currentcell);
            if (dtoString != null)
                return true;

            return false;
        }

        public static string TryGetString_NullFromReaderImediately(SqlDataReader reader, string Currentcell)
        {
            return clsCommenFunctions.FromSQLToString(reader, Currentcell);
        }

        public static bool? TryGetBoolFromReaderImediately(SqlDataReader reader, string Currentcell)
        {
            return clsCommenFunctions.FromSQLToBOOL(reader, Currentcell);
        }

        public static bool TryGetDecimalFromReaderImediately(SqlDataReader reader, string ColumnName, out decimal Decimal)
        {
            Decimal = -1m;
            decimal? value = clsCommenFunctions.FromSQLToDecimal(reader, ColumnName);
            if (value.HasValue)
            {
                Decimal = value.Value;
                return true;
            }
            else
                return false;
        }

        public static bool TryGetDatetimeFromReaderImediately(SqlDataReader reader, string ColumnName, out DateTime value)
        {
            DateTime? dateime = clsCommenFunctions.FromSQLToDateTime(reader, ColumnName);
            if (dateime.HasValue)
            {
                value = dateime.Value;
                return true;
            }
            value = dateime.Value;
            return false;
        }

        public static DateTime? TryGetNULL_DateTime(SqlDataReader reader, string ColumnName)
        {
            return clsCommenFunctions.FromSQLToDateTime(reader, ColumnName);
        }

        public static bool TryGetsByteFromReaderImediately(SqlDataReader reader, string CurrentCell, out sbyte valuesByte)
        {
            valuesByte = -1;
            byte? TempByte = clsCommenFunctions.FromSQLToByte(reader, CurrentCell);
            if (TempByte.HasValue)
            {
                if (TempByte.Value < 0)
                {
                    valuesByte = (sbyte)TempByte.Value;
                    return true;
                }
            }

            return false;
        }

        public static bool TryGetByteFromReaderImediately(SqlDataReader reader, string CurrentCell, out byte valueByte)
        {
            valueByte = 254;
            byte? TempByte = clsCommenFunctions.FromSQLToByte(reader, CurrentCell);
            if (TempByte.HasValue )
            {
                valueByte = TempByte.Value;
                return true;
            }

            return false;
        }

        public static int? TryGetNULL_Int (SqlDataReader reader, string CurrentCell)
        {
            return clsCommenFunctions.FromSQLToInt(reader, CurrentCell);
        }

        public static bool TryGetEnum_Int <T> (SqlDataReader reader, string CurrentCell, out T result, T fallback)
            where T : struct, Enum
        {
            result = fallback;

            if(!TryGetIntFromReaderImmediately(reader, CurrentCell, out int Enum_int))
                return false;

            if(Enum.IsDefined(typeof(T), Enum_int))
            {
                result = (T)Enum.ToObject(typeof(T), Enum_int);
                return true;
            }

            result = fallback;
            return false;
        }

        public static bool TryGetEnum_Byte <T> (SqlDataReader reader, string CurrentCell, out T result, T fallback)
            where T : struct, Enum
        {
            result = fallback;

            if(!TryGetByteFromReaderImediately(reader, CurrentCell, out byte Enum_int))
                return false;

            if(Enum.IsDefined(typeof(T), Enum_int))
            {
                result = (T)Enum.ToObject(typeof(T), Enum_int);
                return true;
            }

            result = fallback;
            return false;
        }
    }
}
