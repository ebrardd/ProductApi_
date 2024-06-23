using ProductApi_.Models;
using ProductApi_.Repositories;
using ProductApi_.V1.Controller;

namespace ProductApi_.Services
{

    public class Service : IService
    {
        private readonly IRepository _repository;
        public Service(IRepository repository)
        {
            _repository = repository;
        }
        public List<Product> GetProduct() // Dönüş türünü List<Product> olarak değiştir
        {
            List<Product> products = _repository.GetProduct();
            ProcessProducts(products);
            return products;
        }

        public void ProcessProducts(List<Product> products)
        {
            Thread thread = new Thread(() =>
            {
                foreach (var product in products)
                {
                    // Ürün işlemleri
                }
            });
            thread.Start();
        }
        public async Task<Product> GetProductById(int id) // Bu metodu implement edin
        {
            return await _repository.GetProductById(id);
        }
        public async Task<Product> UpdateProduct(int id, Product product)
        {
            var updatedProduct = await _repository.UpdateProduct(id, product);
            return updatedProduct;
        }
        public async void CreateOrder(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            // Order ve içindeki ürünlerin doğrulaması (örneğin, stok kontrolü)
            foreach (var product in order.Products)
            {
                var existingProduct = await  _repository.GetProductById(product.Id);
                if (existingProduct == null || existingProduct.Stock < product.Stock)
                {
                    throw new InvalidOperationException("Product is not available or insufficient stock.");
                }
                // Stok güncelleme
                existingProduct.Stock -= product.Stock;
                _repository.UpdateProduct(existingProduct.Id, existingProduct);
            }
            // Siparişi veritabanına ekle
            _repository.AddOrder(order);
        }
        public Product GetProducts()
        {
            throw new NotImplementedException();
        }

        public Task<Product> AddProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Product> DeleteProduct(int id, Product product)
        {
            throw new NotImplementedException();
        }

        public void CheckStockAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Order AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public class InventoryManager
        {
            public delegate void StockLowHandler(Product product);

            public event StockLowHandler OnStockLow;

            public InventoryManager(StockLowHandler handler)
            {
                
                OnStockLow += handler;
            }
            public void CheckStock(Product product)
            {
                if (product.Stock < 10)
                {
                    OnStockLow?.Invoke(product);
                }
            }
        }
    } 
}
