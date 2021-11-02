using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using Worker.Order.Read.Repository.Interfaces;

namespace Worker.Order.Read.Repository
{
    public class LogsRepository : ILogsRepository
    {
        private readonly SqlConnection sqlConnection;

        public LogsRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public int LogRead(string fileName)
        {
            #region SQL

            var dataTable = new DataTable();

            var query =
            $@" DECLARE @OrderRead INT;

                INSERT INTO Worker_Order_Read VALUES ('{fileName}', GETDATE(), 1);
                
                SET @OrderRead = @@IDENTITY;
                
                SELECT @OrderRead AS OrderRead;";

            var command = new SqlCommand(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                sqlConnection.Open();

                var adapter = new SqlDataAdapter(command);

                adapter.Fill(dataTable);

                return (int)dataTable.Rows[0]["OrderRead"];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }

            #endregion SQL
        }

        public void LogRead(string message, int logRead)
        {
            #region SQL

            var query = $@"INSERT INTO Worker_Order_Read_Log VALUES ('{message}', {logRead}, GETDATE()), 1;";

            var command = new SqlCommand(query, sqlConnection)
            {
                CommandType = CommandType.Text
            };

            try
            {
                sqlConnection.Open();

                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }

            #endregion SQL
        }
    }
}
