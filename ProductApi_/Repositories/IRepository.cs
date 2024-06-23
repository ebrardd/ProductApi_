using Microsoft.AspNetCore.Mvc;
using ProductApi_.Models;

namespace ProductApi_.Repositories
{
    public interface IRepository
    {

       // public Product GetProduct();
   
       public List<Product> GetProduct();
        public Task<Product> GetProductById(int id);
        public Task<Product> AddProductAsync(Product product);
        public Task<Product> UpdateProduct(int id ,Product product);
        public Order AddOrder(Order order);//repo
        public Task<Product> DeleteProduct(int id, Product product);
        public Product CreateOrder([FromBody] Order order);
        public Task CheckStockAsync(int productId);
    }
}
