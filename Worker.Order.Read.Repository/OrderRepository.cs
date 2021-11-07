using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Worker.Order.Read.Models.ViewModels;
using Worker.Order.Read.Repository.Interfaces;

namespace Worker.Order.Read.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        public bool CheckPendingOrders()
        {
            #region SQL

            try
            {
                var query =
                @"  SELECT 1 FROM   Worker_Order_Pre_Order_Read         AS PO
                    INNER JOIN      Worker_Order_Pre_Order_Items_Read   AS POI
                        ON PO.Pre_Order_Read_ID = POI.Pre_Order_Read_ID
                    WHERE PO.Active  = 1
                      AND POI.Active = 1";

                using SqlConnection conn = new SqlConnection(_connectionString);
                return conn.QueryFirstOrDefault<bool>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public int GetNextPendingOrder()
        {
            #region SQL

            try
            {
                var query =
                @"  SELECT		TOP 1 PO.Pre_Order_Read_ID
                    FROM		Worker_Order_Pre_Order_Read			AS PO
                    INNER JOIN	Worker_Order_Pre_Order_Items_Read	AS POI
                    	ON PO.Pre_Order_Read_ID = POI.Pre_Order_Read_ID
                    WHERE PO.Active  = 1
                      AND POI.Active = 1";

                using SqlConnection conn = new SqlConnection(_connectionString);
                return conn.QueryFirstOrDefault<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public OrderInfoViewModel GetNextPendingOrderInfo(int pendingOrderNumber)
        {
            #region SQL

            try
            {
                var query =
                $@" SELECT		 PO.Pre_Order_Read_ID
                    			,Pre_Order_Order_Number
                    			,Pre_Order_Order_Date
                    			,Pre_Order_Delivery_Notes
                    			,Pre_Order_Order_Shipping_Name
                    			,Pre_Order_Order_Shipping_Street
                    			,Pre_Order_Order_Shipping_City
                    			,Pre_Order_Order_Shipping_State
                    			,Pre_Order_Order_Shipping_Zip
                    			,Pre_Order_Order_Shipping_Country
                    			,Pre_Order_Order_Billing_Name
                    			,Pre_Order_Order_Billing_Street
                    			,Pre_Order_Order_Billing_City
                    			,Pre_Order_Order_Billing_State
                    			,Pre_Order_Order_Billing_Zip
                    			,Pre_Order_Order_Billing_Country
                    FROM		Worker_Order_Pre_Order_Read			AS PO
                    INNER JOIN	Worker_Order_Pre_Order_Items_Read	AS POI
                    	ON PO.Pre_Order_Read_ID = POI.Pre_Order_Read_ID
                    WHERE PO.Active  = 1
                      AND POI.Active = 1
                      AND PO.Pre_Order_Read_ID = {pendingOrderNumber}";

                using SqlConnection conn = new SqlConnection(_connectionString);
                return conn.QueryFirstOrDefault<OrderInfoViewModel>(query, new { pendingOrderNumber });
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public IEnumerable<ItemViewModel> GetNextPendingOrderItems(int pendingOrderNumber)
        {
            #region SQL

            try
            {
                var query =
                $@" SELECT		 Pre_Order_Item_Read_ID
                    			,Pre_Order_Item_Part_Number
                    			,Pre_Order_Item_Product_Name
                    			,Pre_Order_Item_Quantity
                    			,Pre_Order_Item_Price
                    			,Pre_Order_Item_Comment
                    FROM		Worker_Order_Pre_Order_Read			AS PO
                    INNER JOIN	Worker_Order_Pre_Order_Items_Read	AS POI
                    	ON PO.Pre_Order_Read_ID = POI.Pre_Order_Read_ID
                    WHERE PO.Active  = 1
                      AND POI.Active = 1
                      AND PO.Pre_Order_Read_ID = {pendingOrderNumber}";

                using SqlConnection conn = new SqlConnection(_connectionString);
                return conn.Query<ItemViewModel>(query);
            }
            catch (Exception)
            {
                throw;
            }
            
            #endregion SQL
        }

        public bool OrderNumberExists(int orderNumber)
        {
            #region SQL

            try
            {
                var query = $@"SELECT 1 FROM Worker_Order WHERE Order_Number = '{orderNumber}';";

                using var conn = new SqlConnection(_connectionString);
                return conn.QueryFirstOrDefault<bool>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public int InsertShippingAddress(Entity.Address shippingAddress)
        {
            #region SQL

            try
            {
                var query =
                $@" DECLARE @ShippingID INT;

                    INSERT INTO Worker_Order_Shipping_Address
                    VALUES
                    (
                         '{shippingAddress.Name}'
                        ,'{shippingAddress.Street}'
                        ,'{shippingAddress.City}'
                        ,'{shippingAddress.State}'
                        ,'{shippingAddress.Zip}'
                        ,'{shippingAddress.Country}'
                        ,GETDATE()
                        ,1
                    );

                    SET @ShippingID = @@IDENTITY;

                    SELECT @ShippingID;";

                using var conn = new SqlConnection(_connectionString);
                return conn.ExecuteScalar<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public int InsertBillingAddress(Entity.Address billingAddress)
        {
            #region SQL

            try
            {
                var query =
                $@" DECLARE @BillingID INT;

                    INSERT INTO Worker_Order_Billing_Address
                    VALUES
                    (
                         '{billingAddress.Name}'
                        ,'{billingAddress.Street}'
                        ,'{billingAddress.City}'
                        ,'{billingAddress.State}'
                        ,'{billingAddress.Zip}'
                        ,'{billingAddress.Country}'
                        ,GETDATE()
                        ,1
                    );

                    SET @BillingID = @@IDENTITY;
                    
                    SELECT @BillingID;";

                using var conn = new SqlConnection(_connectionString);
                return conn.ExecuteScalar<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public int InsertOrderInfo(Entity.Order order, int shippingID, int billingID)
        {
            #region SQL

            try
            {
                var query =
                $@"  DECLARE @OrderID INT;

                     INSERT INTO Worker_Order
                     VALUES
                     (
                          '{order.OrderNumber}'
                         ,'{order.OrderDate}'
                         ,{shippingID}
                         ,{billingID}
                         ,'{order.DeliveryNotes}'
                         ,GETDATE()
                         ,1
                     );

                     SET @OrderID = @@IDENTITY;

                     SELECT @OrderID;";

                using var conn = new SqlConnection(_connectionString);
                return conn.ExecuteScalar<int>(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public void InsertItems(List<Entity.Item> items, int orderID)
        {
            #region SQL

            try
            {
                foreach (var item in items)
                {
                    var query =
                    $@" INSERT INTO Worker_Order_Item
                    VALUES
                    (
                         '{item.PartNumber}'
                        ,'{item.ProductName}'
                        ,{item.Quantity}
                        ,'{item.Price}'
                        ,'{item.Comment}'
                        ,{orderID}
                        ,GETDATE()
                        ,1
                    );";

                    using var conn = new SqlConnection(_connectionString);
                    conn.Execute(query);
                }
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }

        public void DeactivatePreOrder(int preOrderID)
        {
            #region SQL

            try
            {
                var query =
                $@" UPDATE Worker_Order_Pre_Order_Read SET Active = 0 WHERE Pre_Order_Read_ID = {preOrderID};
                    UPDATE Worker_Order_Pre_Order_Items_Read SET Active = 0 WHERE Pre_Order_Read_ID = {preOrderID};";

                using var conn = new SqlConnection(_connectionString);
                conn.Execute(query);
            }
            catch (Exception)
            {
                throw;
            }

            #endregion SQL
        }
    }
}
