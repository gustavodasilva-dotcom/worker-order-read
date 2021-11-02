using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using Worker.Order.Read.Entity;
using Worker.Order.Read.Repository.Interfaces;

namespace Worker.Order.Read.Repository
{
    public class ReadRepository : IReadRepository
    {
        private readonly SqlConnection sqlConnection;

        public ReadRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public int InsertRead(int logRead, Entity.Order order)
        {
            #region SQL

            var dataTable = new DataTable();

            var query =
            $@" DECLARE @PreOrderID INT;

                INSERT INTO Worker_Order_Pre_Order_Read
                VALUES
                (
                	 '{order.OrderNumber}'
                	,'{order.OrderDate:yyyy-MM-dd}'
                    ,'{order.DeliveryNotes}'
                	,'{order.Shipping.Name}'
                	,'{order.Shipping.Street}'
                	,'{order.Shipping.City}'
                	,'{order.Shipping.State}'
                	,'{order.Shipping.Zip}'
                	,'{order.Shipping.Country}'
                	,'{order.Billing.Name}'
                	,'{order.Billing.Street}'
                	,'{order.Billing.City}'
                	,'{order.Billing.State}'
                	,'{order.Billing.Zip}'
                    ,'{order.Billing.Country}'
                	,{logRead}
                    ,1
                );
                
                SET @PreOrderID = @@IDENTITY;
                
                SELECT @PreOrderID AS PreOrderID;";

            var command = new SqlCommand(query, sqlConnection);

            command.CommandType = CommandType.Text;

            try
            {
                sqlConnection.Open();

                var adapter = new SqlDataAdapter(command);

                adapter.Fill(dataTable);

                return (int)dataTable.Rows[0]["PreOrderID"];
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

        public void InsertReadItems(int readId, Entity.Order order)
        {
            #region SQL

            foreach (Item item in order.Items)
            {
                var query =
                $@" INSERT INTO Worker_Order_Pre_Order_Items_Read
                    VALUES
                    (
                    	 '{item.PartNumber}'
                    	,'{item.ProductName}'
                    	,'{item.Quantity}'
                    	,'{item.Price}'
                    	,'{item.Comment}'
                    	,{readId}
                        ,1
                    );";

                var command = new SqlCommand(query, sqlConnection);

                command.CommandType = CommandType.Text;

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
            }

            #endregion SQL
        }
    }
}
