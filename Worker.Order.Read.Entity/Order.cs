using System;
using System.Collections.Generic;

namespace Worker.Order.Read.Entity
{
    public class Order
    {
        public int OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public Address Shipping { get; set; }

        public Address Billing { get; set; }

        public List<Item> Items { get; set; }

        public string DeliveryNotes { get; set; }
    }
}
