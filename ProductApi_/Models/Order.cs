namespace ProductApi_.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public decimal TotalAmount
        {
            get
            {
                decimal total = 0;

                foreach (var product in Products)
                {
                    total += product.Price * product.Stock;
                }
                return total; 

            }
        }
    }
}
