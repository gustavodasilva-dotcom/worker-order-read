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
            return false;
        }
    }
}
