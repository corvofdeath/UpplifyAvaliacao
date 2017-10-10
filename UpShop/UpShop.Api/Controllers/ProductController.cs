using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UpShop.Dominio.Interfaces;
using Microsoft.Extensions.Logging;
using UpShop.Dominio.Entitys;
using System.Threading.Tasks;
using UpShop.Api.Utils;
using MongoDB.Bson;
using UpShop.Api.Models;

namespace UpShop.Api.Controllers
{
    /// <summary>
    /// Must be authorized for all operations
    /// </summary>
    [Authorize]
    [Produces("application/json")]
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> logger;
        private readonly IRepository repository;

        public ProductController(IRepository _repository, ILogger<ProductController> _logger)
        {
            repository = _repository;
            logger = _logger;
        }

        /// <summary>
        /// Get all products. Must be authorized
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var Products = await repository.GetAllAsync<Product>();

            return Ok(Products);
        }

        /// <summary>
        /// Get a specific product. Must be authorized
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var objectId = new ObjectId(id);
            var product = await repository.GetAsync<Product>(c => c.Id == objectId);

            if(product == null)
            {
                logger.LogWarning(LoggingEvents.GetItemNotFound, "Get failed, product not found. Id: {0}", product.Id);
                return BadRequest(new ErrorResponse("[ERROR] - Product not found!"));
            }

            return Ok(product);
        }

        /// <summary>
        /// Update a specific product. Must be authorized
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PUT /update
        ///     {
        ///        "id": 0,
        ///        "price": "newPrice:
        ///     }
        ///
        /// </remarks>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ProductModel product)
        {
            // Verify if deserialize function return ok
            if (product == null)
            {
                logger.LogWarning(LoggingEvents.DeserializeJSON, "Fail to deserialize JSON to Product type.");
                return BadRequest(new ErrorResponse("[ERROR] - In Parse JSON! Verify your propertys."));
            }

            // if is no Id in the Product send
            if (product.Id == ObjectId.Empty)
            {
                logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Try to update product without id.");
                return BadRequest(new ErrorResponse("[ERROR] - Must provide the Id for the product to update!"));
            }

            var entity = await repository.GetByIdAsync<Product>(product.Id);

            if (entity == null)
            {
                logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Update failed, product not found. Id: {0}", product.Id);
                return BadRequest(new ErrorResponse("[ERROR] - Product not found!"));
            }

            logger.LogInformation(LoggingEvents.MappingOperation, "Mapping DTO: ProductModel to Entity: Product. Id: {0}", entity.Id);
            product.MapperProperties(entity);

            logger.LogInformation(LoggingEvents.UpdateItem, "Update product. Id: {0}", entity.Id);
            await repository.UpdateAsync(entity);

            return Ok();

        }

        /// <summary>
        /// Remove a product. Must be authorized
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            var objectId = new ObjectId(id);

            var exist = await repository.ExistAsync<Product>(c => c.Id == objectId);

            if (!exist)
            {
                logger.LogWarning(LoggingEvents.GetItemNotFound, "Try to remove product, Id not found. Id: {0}", id);
                return BadRequest(new ErrorResponse("[ERROR] - Product not found!"));
            }

            await repository.RemoveAsync<Product>(objectId);

            return Ok();
        }

        /// <summary>
        /// Create a product. Must be authorized
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /create
        ///     {
        ///        "name": "cinto",
        ///        "description": "cinto muito",
        ///        "quantity": "1",
        ///        "price": "50",
        ///     }
        ///
        /// </remarks>        
        /// <param name="newProduct"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Product newProduct)
        {
            // Verify if deserialize function return ok
            if (newProduct == null)
            {
                logger.LogWarning(LoggingEvents.DeserializeJSON, "Fail to deserialize JSON to Product type.");
                return BadRequest(new ErrorResponse("[ERROR] - In Parse JSON! Verify your propertys."));
            }

            // Verify if the product alredy exist
            logger.LogInformation(LoggingEvents.VerifyItemExist, "Verify if product exist before creation. Name: {0}", newProduct.Name);
            bool Product = await repository.ExistAsync<Product>(c => c.Name == newProduct.Name);

            if (Product)
            {
                logger.LogWarning(LoggingEvents.ItemAlreadyExist, "Try to create a product that already exist. Email: {0}", newProduct.Name);
                return BadRequest(new ErrorResponse("[ERROR] - Product already exists!"));
            }

            logger.LogInformation(LoggingEvents.InsertItem, "Insert new product. Email: {0}", newProduct.Name);
            await repository.CreateAsync(newProduct);

            return Ok(newProduct);
        }
    }
}