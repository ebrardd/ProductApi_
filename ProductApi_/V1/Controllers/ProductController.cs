using ProductApi_.Models;
using ProductApi_.Repositories;
using ProductApi_.Services;
using ProductApi_.V1.Models.ResponseModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductApi_.V1.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase

    {
        private readonly ILogger<ProductController> _logger;
        private readonly IService _service;
        private readonly IRepository _repository;

        public ProductController(ILogger<ProductController> logger, IService service)
        {
            _logger = logger;
            _service = service;

        }
        [HttpGet("Products")] //sunucudanverialmakvesorguyapmak
        public IActionResult GetProducts()
        {
            var products = _service.GetProducts();

            if (products is null|| !products.Any())
            {
                return NotFound();
            } 

            else

                return Ok(products);
        }
        [HttpGet("Products/{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _service.GetProductById(id);
            return Ok(product);
        }
        [HttpPost("Products")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (product == null)
            {
                return BadRequest("Product is null.");
            }

            await _repository.AddProductAsync(product);
            return Ok("Product added successfully.");
        }
        [HttpPut("Products/{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
        {
            try
            {
                if (id != product.Id)
                    return BadRequest("Product Id mismatch");

                var ProductToUpdate = await _service.GetProductById(id);
                
                if (ProductToUpdate is null)
                    return NotFound($"Product with Id = {id} not found");
            

                var UpdatedProduct = await _service.UpdateProduct(id, product);

                if (UpdatedProduct != null)
                {
                    return Ok(UpdatedProduct);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }

        [HttpDelete("Products/{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id, Product product)
        {
            try
            {
                var productToDelete = await _service.GetProductById(id);

                if (productToDelete == null)
                {
                    return NotFound($" {id} not found");
                }

                var deletedproduct = await _service.DeleteProduct(id, product);

                if (deletedproduct != null)
                {
                    return Ok(deletedproduct);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error deleting data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
        [HttpPost("Orders")]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return BadRequest("Invalid order.");
            }

            try
            {
                _service.CreateOrder(order); 
                return Ok("Order created successfully.");
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        public async Task CheckStockAsync(int productId)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync($"https://api.example.com/stock/{productId}");
            string responseData = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseData);
        }


    }
}

