using ProductApi_.Models;

namespace ProductApi_.Extension.CustomExceptionMiddleWare
{
    public class ErrorLoggingMiddleWareExtension
    {
        public void AddProductToOrder(Order order, Product product, int quantity)
        {
            try
            {
                if (product.Stock < quantity)
                {
                    throw new Exception("Yeterli stok yok.");
                }
                else
                {
                    product.Stock -= quantity;
                    order.Products.Add(product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }




    }
}
