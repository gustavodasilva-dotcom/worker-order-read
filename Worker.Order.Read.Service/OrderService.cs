using System;
using System.Collections.Generic;
using Worker.Order.Read.Repository.Interfaces;
using Worker.Order.Read.Service.Interfaces;

namespace Worker.Order.Read.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public bool CheckPendingOrders()
        {
            try
            {
                return _orderRepository.CheckPendingOrders();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Worker.Order.Read.Entity.Order GetNextPendingOrder()
        {
            try
            {
                var pendingOrderId = _orderRepository.GetNextPendingOrder();

                var pendingOrderInfo = _orderRepository.GetNextPendingOrderInfo(pendingOrderId);

                var pendingOrderItems = _orderRepository.GetNextPendingOrderItems(pendingOrderId);

                var order = new Worker.Order.Read.Entity.Order
                {
                    PreOrderID = pendingOrderInfo.Pre_Order_Read_ID,
                    OrderNumber = Convert.ToInt32(pendingOrderInfo.Pre_Order_Order_Number),
                    OrderDate = Convert.ToDateTime(pendingOrderInfo.Pre_Order_Order_Date),
                    Shipping = new Entity.Address
                    {
                        Name = pendingOrderInfo.Pre_Order_Order_Shipping_Name,
                        Street = pendingOrderInfo.Pre_Order_Order_Shipping_Street,
                        City = pendingOrderInfo.Pre_Order_Order_Shipping_City,
                        State = pendingOrderInfo.Pre_Order_Order_Shipping_State,
                        Zip = Convert.ToInt32(pendingOrderInfo.Pre_Order_Order_Shipping_Zip),
                        Country = pendingOrderInfo.Pre_Order_Order_Shipping_Country
                    },
                    Billing = new Entity.Address
                    {
                        Name = pendingOrderInfo.Pre_Order_Order_Billing_Name,
                        Street = pendingOrderInfo.Pre_Order_Order_Billing_Street,
                        City = pendingOrderInfo.Pre_Order_Order_Billing_City,
                        State = pendingOrderInfo.Pre_Order_Order_Billing_State,
                        Zip = Convert.ToInt32(pendingOrderInfo.Pre_Order_Order_Billing_Zip),
                        Country = pendingOrderInfo.Pre_Order_Order_Billing_Country
                    },
                    DeliveryNotes = pendingOrderInfo.Pre_Order_Delivery_Notes
                };

                var items = new List<Entity.Item>();

                foreach (var item in pendingOrderItems)
                {
                    items.Add(new Entity.Item
                    {
                        PartNumber = item.Pre_Order_Item_Part_Number,
                        ProductName = item.Pre_Order_Item_Product_Name,
                        Quantity = Convert.ToInt32(item.Pre_Order_Item_Quantity),
                        Price = Convert.ToDouble(item.Pre_Order_Item_Price),
                        Comment = item.Pre_Order_Item_Comment
                    });
                }

                order.Items = items;

                return order;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool OrderNumberExists(Worker.Order.Read.Entity.Order order)
        {
            try
            {
                return _orderRepository.OrderNumberExists(order.OrderNumber);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ProcessOrder(Worker.Order.Read.Entity.Order order)
        {
            try
            {
                var shippingID = _orderRepository.InsertShippingAddress(order.Shipping);

                if (shippingID != 0)
                {
                    var billingID = _orderRepository.InsertBillingAddress(order.Billing);

                    if (billingID != 0)
                    {
                        var orderID = _orderRepository.InsertOrderInfo(order, shippingID, billingID);

                        if (orderID != 0)
                        {
                            _orderRepository.InsertItems(order.Items, orderID);

                            DeactivatePreOrder(order.PreOrderID);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeactivatePreOrder(int preOrderID)
        {
            try
            {
                _orderRepository.DeactivatePreOrder(preOrderID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
