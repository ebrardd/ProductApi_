using Microsoft.AspNetCore.Mvc;
using ProductApi_.Models;


namespace ProductApi_.Services
{
    public interface IService 
    {
        public Product GetProducts();
        public Task<Product> GetProductById(int id);
        public Task<Product> UpdateProduct(int id, Product product);
        public Task<Product> AddProductAsync( Product product);
        public Task<Product> DeleteProduct(int id, Product product);
        public void CreateOrder(Order order);
        public void CheckStockAsync(int Id);//productId
        public Order AddOrder(Order order);
    }
}