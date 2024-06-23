
using ProductApi_.Models;

namespace ProductApi_.Configs
{
    public class ErrorHandlingSettings
    {
        public void HandleStockLow(Product product)
        {
            Console.WriteLine($"Warning: The stock for product {product.Name} is low (current stock: {product.Stock}).");
        }
    }
}
