using ProductApi_.Models;
using System.Text.Json;
using MongoDB.Driver;
using ProductApi_.Configs;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;

namespace ProductApi_.Repositories
{
    public class Repository : IRepository

    {
        private readonly IMongoCollection<Models.Product> productCollection;
        private readonly IMongoCollection<Order> orderCollection;
        private readonly ILogger<Repository> _logger;
        private readonly IMongoDBSettings _mongodbSettings;

        public Repository(IMongoDBSettings mongoDBSettings, ILogger<Repository> logger)
        {
            _mongodbSettings = mongoDBSettings;
            var client = new MongoClient(mongoDBSettings.ConnectionUri);
            var database = client.GetDatabase(mongoDBSettings.DatabaseName);
            productCollection = database.GetCollection<Product>(mongoDBSettings.CollectionName1);
            orderCollection = database.GetCollection<Order>(mongoDBSettings.CollectionName2);
            try
            {
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                _logger.LogInformation("MongoDatabase connection has been succesfull");
                Console.WriteLine("Pinged your deployment. You successfully connected to MongoDB!");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ERROR!Something went wrong");
                Console.WriteLine(ex);
            }
        }
        public List<Product> GetProduct()
        {
            var productList = productCollection.Find(Builders<Product>.Filter.Empty).ToList();
            return productList;
        }
        public async Task<Product> GetById(int Id)
        {
            var filter = Builders<Product>.Filter.Eq(product => product.Id, Id);
            return productCollection.Find(filter).FirstOrDefault();
        }
        public async Task<Product> AddProductAsync(Product product)
        {
            await productCollection.InsertOneAsync(product);
            _logger.LogInformation("Product has been added");
            ////InsertOne yeni veri eklemek veya mevcut veriyi güncellemek için kullanılan bir SQL komutu
            return product;
        }
        public async Task<Product> DeleteProduct(int id, Product product)
        {

            var filter = Builders<Product>.Filter.Eq(m => m.Id, id);
            var result = await productCollection.FindOneAndDeleteAsync(filter);

            return result;
        }
        public async Task<Product> UpdateProduct(int id, Product product)
        {
            var filter = Builders<Product>.Filter.Eq(e => e.Id, product.Id);
            var update = Builders<Product>.Update
                .Set(e => e.Name, product.Name)
                .Set(e => e.Price, product.Price)
                .Set(e => e.Stock, product.Stock);
            var options = new FindOneAndUpdateOptions<Product>
            {
                ReturnDocument = ReturnDocument.After
            };

            var updatedgoods = await productCollection.FindOneAndUpdateAsync(filter, update, options);

            return updatedgoods;
        }

        public Order AddOrder(Order order) //dönüs türü önemli Cannot implicitly convert type 'Order' to 'Product'
        {
            if (order == null)
            {
                _logger.LogError("Order cannot be null");
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            }

            orderCollection.InsertOne(order);
            _logger.LogInformation("Order has been created with ID: {OrderId}", order.OrderId);
            return order;
        }
        public Order CreateOrder(Order order)
        {
            if (order == null)
            {
                _logger.LogError("Order cannot be null");
                throw new ArgumentNullException(nameof(order), "Order cannot be null");
            }

            foreach (var product in order.Products)
            {
                var filter = Builders<Order>.Filter.ElemMatch(o => o.Products, p => p.Name == product.Name);
                var existingOrder = orderCollection.Find(filter).FirstOrDefault();

                if (existingOrder != null)
                {
                    var existingProduct = existingOrder.Products.First(p => p.Name == product.Name);
                    existingProduct.Stock += product.Stock;
                    existingProduct.Price = product.Price;

                    var update = Builders<Order>.Update.Set(o => o.Products[-1], existingProduct);
                    orderCollection.UpdateOne(filter, update);
                    _logger.LogInformation("Order has been updated with Product: {ProductName}", product.Name);
                }
                else
                {
                    orderCollection.InsertOne(order);
                    _logger.LogInformation("Order has been created with ID: {OrderId}", order.OrderId);
                }
            }
            return order;
        }
        public async Task<int> CheckStockAsync(int productId)
        {
            var filter = Builders<Order>.Filter.ElemMatch(o => o.Products, p => p.Id == productId);
            var orders = await orderCollection.Find(filter).ToListAsync();
            int totalStock = orders.Sum(o => o.Products.First(p => p.Id == productId).Stock);

            _logger.LogInformation("Total stock for Product ID: {ProductId} is {TotalStock}", productId, totalStock);
            return totalStock;
        }


        public Task<Product> GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        Product IRepository.CreateOrder(Order order)
        {
            throw new NotImplementedException();
        }

        Task IRepository.CheckStockAsync(int productId)
        {
            throw new NotImplementedException();
        }
    }
}


