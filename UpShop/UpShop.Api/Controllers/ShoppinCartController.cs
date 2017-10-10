using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpShop.Api.Models;
using UpShop.Api.Utils;
using UpShop.Dominio.Entitys;
using UpShop.Dominio.Interfaces;

namespace UpShop.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/shoppingcart")]
    public class ShoppingCartController : Controller
    {
        private readonly ILogger<UserController> logger;
        private readonly IRepository repository;

        public ShoppingCartController(IRepository _repository, ILogger<UserController> _logger)
        {
            repository = _repository;
            logger = _logger;
        }

        /// <summary>
        /// Buy products and create historic. Must be authorized
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /
        ///     {
        ///        "userId": "0",
        ///        "products": [
        ///             {
        ///                 "id": "0",
        ///                 "quantity": "5"
        ///             }
        ///         ],
        ///         description: "cinto que comprei demais"
        ///     }
        ///
        /// </remarks>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Buy([FromBody] ShoppingCartModel shoppingCart)
        {
            // Verify if deserialize function return ok
            if (shoppingCart == null)
            {
                logger.LogWarning(LoggingEvents.DeserializeJSON, "Fail to deserialize JSON to ShoppingCartModel type.");
                return BadRequest(new ErrorResponse("[ERROR] - In Parse JSON! Verify your propertys."));
            }

            // Get a list of ids to use in complex querys
            var productsId = shoppingCart.Products.Select(c => c.Id);

            var user = await repository.GetAsync<User>(c => c.Id == shoppingCart.UserId);
            if(user == null)
            {
                logger.LogWarning(LoggingEvents.GetItemNotFound, "Get failed, user not found. Id: {0}", user.Id);
                return BadRequest(new ErrorResponse("[ERROR] - User not found!"));
            }

            logger.LogInformation(LoggingEvents.ListItems, "Get products from shopping cart");
            // Get a list of products using a complex query (producs ids)
            var products = await repository.GetAllAsync<Product>(c => productsId.Contains(c.Id));
            // Make a list of products details and update they quantitys
            var productsDetails = GetProductDetails(products, shoppingCart);

            // Create a new historic
            logger.LogInformation(LoggingEvents.GenerateItems, "Creating a new historic of shopping for User: {0}", user.Email);
            Historic newHistoric = new Historic(shoppingCart.Description);
            newHistoric.AddProductsDetail(productsDetails);
            newHistoric.CalcTotal();
            newHistoric.GenerateId();

            //Subtract quantity of products in database
            await UpdateProductsQuantitys(newHistoric.Products.Select(c => c.Product));

            logger.LogInformation(LoggingEvents.InsertItem, "Insert new historic. Id: {0}", newHistoric.Id);
            user.Historics.Add(newHistoric);
            await repository.UpdateAsync<User>(user);

            return Ok(newHistoric);
        }

        //================================ Helpers =============================================================

        /// <summary>
        /// Execute query to transform and merge a list of product with they respective quantitys.
        /// </summary>
        /// <param name="products"></param>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        private IEnumerable<ProductDetail> GetProductDetails(IEnumerable<Product> products, ShoppingCartModel shoppingCart)
        {
            // TODO: otimize this code
            return products.Select(c => new ProductDetail(c, shoppingCart.Products.First(a => a.Id == c.Id).Quantity));
        }

        /// <summary>
        /// Propagate the changes in historic products quantitys for the real entity
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private async Task UpdateProductsQuantitys(IEnumerable<Product> products)
        {
            // we don't have a 'UpdateMany' method because we use a replace method instead.
            // this is because is hard to dynamic set a list of property for update in a mongo db driver typed.
            foreach (var product in products)
            {
                await repository.UpdateAsync(product);
            }
        }
    }
}