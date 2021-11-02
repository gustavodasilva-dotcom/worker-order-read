namespace Worker.Order.Read.Entity
{
    public class Item
    {
        public string PartNumber { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string Comment { get; set; }
    }
}
