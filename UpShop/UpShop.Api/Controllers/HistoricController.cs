using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System.Threading.Tasks;
using UpShop.Dominio.Entitys;
using UpShop.Dominio.Interfaces;
using UpShop.Api.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;

namespace UpShop.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Historic")]
    public class HistoricController : Controller
    {
        private readonly ILogger<ProductController> logger;
        private readonly IRepository repository;

        public HistoricController(IRepository _repository, ILogger<ProductController> _logger)
        {
            repository = _repository;
            logger = _logger;
        }

        /// <summary>
        /// Get all historic from specific user. Must be authorized
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAllHistoricFromUser(string id)
        {
            ObjectId Id = new ObjectId(id);

            var user = await repository.GetAsync<User>(c => c.Id == Id);
            if (user == null)
            {
                logger.LogWarning(LoggingEvents.GetItemNotFound, "Get failed, user not found. Id: {0}", user.Id);
                return BadRequest(new ErrorResponse("[ERROR] - User not found!"));
            }

            return Ok(new { user.Historics });
        }

        /// <summary>
        /// Get a specific historic by transaction id. Must be authorized
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("transaction/{id}")]
        public async Task<IActionResult> GetHistory(string id)
        {
            var Id = new Guid(id);

            var user = await repository.GetAsync<User>(c => c.Historics.Any(a => a.Id == Id));
            if (user == null)
            {
                logger.LogWarning(LoggingEvents.GetItemNotFound, "Get failed, transaction not found. Id: {0}", user.Id);
                return BadRequest(new ErrorResponse("[ERROR] - Transaction not found!"));
            }

            var historic = user.Historics.First(c => c.Id == Id);

            return Ok(historic);
        }
    }
}