using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLayer
{
    public static class Logger
    {
        public static bool LogError(string ConnectionString, string message, string LogLevel = "Info", Exception ex = null, 
            string Source = "UnKnown", int? UserID = null)
        {
            int rowaffected = 0;

            string Query = @"INSERT INTO Logs (LogLevel, Message, Exception, Source, CreatedByUserID)
                             VALUES (@LogLevel, @Message, @Exception, @Source, @CreatedByUserID)";

            SqlConnection Connection = new SqlConnection(ConnectionString);
            SqlCommand command = new SqlCommand(Query, Connection);
            command.Parameters.Add("@LogLevel", System.Data.SqlDbType.NVarChar, 20).Value = LogLevel;
            command.Parameters.Add("@Message", System.Data.SqlDbType.NVarChar, -1).Value = (object) message ?? DBNull.Value;
            command.Parameters.Add("@Exception", System.Data.SqlDbType.NVarChar, -1).Value = (object)ex?.ToString() ?? DBNull.Value;
            command.Parameters.Add("@Source", System.Data.SqlDbType.NVarChar, 100).Value = Source;
            command.Parameters.Add("@CreatedByUserID", System.Data.SqlDbType.Int).Value = (object)UserID ?? DBNull.Value;

            try
            {
                Connection.Open();
                rowaffected = command.ExecuteNonQuery();
            }
            catch
            {
                rowaffected = 0;
            }
            finally
            {
                Connection.Close();
            }

            return (rowaffected > 0); 
        }
    }
}
