using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;
using UpShop.Api.Auth;
using UpShop.Api.Models;
using UpShop.Api.Utils;
using UpShop.Dominio.Entitys;
using UpShop.Dominio.Interfaces;

namespace UpShop.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly IRepository repository;
        private readonly Encryptor encryptor;
        private readonly JwtProvider tokenProvider;
        private readonly ILogger<UserController> logger;

        public UserController(IRepository _repository, JwtProvider _tokenProvider, Encryptor _encryptor, ILogger<UserController> _logger)
        {
            repository = _repository;
            encryptor = _encryptor;
            tokenProvider = _tokenProvider;
            logger = _logger;
        }

        //================================ Authorized methods ===============================================

        /// <summary>
        /// Get all users. Must be authorized
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await repository.GetAllAsync<User>();

            return Ok(users);
        }

        /// <summary>
        /// Get a specific user. Must be authorized
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var objectId = new ObjectId(id);
            var user = await repository.GetAsync<User>(c => c.Id == objectId);

            if (user == null)
            {
                logger.LogWarning(LoggingEvents.GetItemNotFound, "Get failed, user not found. Id: {0}", user.Id);
                return BadRequest(new ErrorResponse("[ERROR] - User not found!"));
            }

            return Ok(user);
        }

        /// <summary>
        /// Update a especific user. Must be authorized
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     PUT /update
        ///     {
        ///        "id": 0,
        ///        "firstName": "newFirstName:
        ///     }
        ///
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserModel user)
        {
            // Verify if deserialize function return ok
            if (user == null)
            {
                logger.LogWarning(LoggingEvents.DeserializeJSON, "Fail to deserialize JSON to User type.");
                return BadRequest(new ErrorResponse("[ERROR] - In Parse JSON! Verify your propertys."));
            }

            // if is no Id in the user send
            if (user.Id == ObjectId.Empty)
            {
                logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Try to update user without id.");
                return BadRequest(new ErrorResponse("[ERROR] - Must provide the Id for the user to update!"));
            }

            var entity = await repository.GetByIdAsync<User>(user.Id);
            if(entity == null)
            {
                logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Update failed, user not found. Id: {0}", user.Id);
                return BadRequest(new ErrorResponse("[ERROR] - User not found!"));
            }
            
            logger.LogInformation(LoggingEvents.MappingOperation, "Mapping DTO: ProductModel to Entity: Product. Id: {0}", entity.Id);
            user.MapperProperties(entity);

            // Update password hash if has changed
            if(user.IsPasswordChanged)
            {
                logger.LogInformation(LoggingEvents.PasswordChanged, "Create a new hash to updated password. Id: {0}", entity.Id);
                var stringfiedCryptedPassword = encryptor.GetEncriptedPassword(entity.Password);
                entity.Password = stringfiedCryptedPassword;
            }

            logger.LogInformation(LoggingEvents.UpdateItem, "Update user. Id: {0}", user.Id);
            await repository.UpdateAsync(entity);

            return Ok();
        }

        /// <summary>
        /// Remove a user. Must be authorized
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            var objectId = new ObjectId(id);

            var exist = await repository.ExistAsync<User>(c => c.Id == objectId);

            if (!exist)
            {
                logger.LogWarning(LoggingEvents.GetItemNotFound, "Try to remove user, Id not found. Id: {0}", id);
                return BadRequest(new ErrorResponse("[ERROR] - User not found!"));
            }

            await repository.RemoveAsync<User>(objectId);

            return Ok();
        }

        //============================== Unauthorized methods ===============================================

        /// <summary>
        /// Create a entity on system
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /create
        ///     {
        ///        "firstName": fulano,
        ///        "lastName": "de tal",
        ///        "password": "123",
        ///        "email": "email@email.com",
        ///        "phone": "1111-1111",
        ///     }
        ///
        /// </remarks>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] User newUser)
        {
            // Verify if deserialize function return ok
            if (newUser == null)
            {
                logger.LogWarning(LoggingEvents.DeserializeJSON, "Fail to deserialize JSON to User type.");
                return BadRequest(new ErrorResponse("[ERROR] - In Parse JSON! Verify your propertys."));
            }

            // Verify if user alredy exist
            logger.LogInformation(LoggingEvents.VerifyItemExist, "Verify if user exist before creation. Email: {0}", newUser.Email);
            bool user = await repository.ExistAsync<User>(c => c.Email == newUser.Email);

            if (user)
            {
                logger.LogWarning(LoggingEvents.ItemAlreadyExist, "Try to create a user that already exist. Email: {0}", newUser.Email);
                return BadRequest(new ErrorResponse("[ERROR] - User already exists!"));
            }

            // Get hash password
            var stringfiedCryptedPassword = encryptor.GetEncriptedPassword(newUser.Password);
            newUser.Password = stringfiedCryptedPassword;

            logger.LogInformation(LoggingEvents.InsertItem, "Insert new user. Email: {0}", newUser.Email);
            await repository.CreateAsync(newUser);

            // Create a new token
            var encodedToken = tokenProvider.CreateEncoded(newUser.FirstName);

            return Ok(new { User = newUser, Token = encodedToken, Expires = DateTime.Now });
        }

        /// <summary>
        /// Get the token from Api
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///     POST /login
        ///     {
        ///        "email": "email@email.com",
        ///        "password": "123"
        ///     }
        ///
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsModel user)
        {
            // Verify if deserialize function return ok
            if (user == null)
            {
                logger.LogWarning(LoggingEvents.DeserializeJSON, "Fail to deserialize JSON to User type.");
                return BadRequest(new ErrorResponse("[ERROR] - In Parse JSON! Verify your propertys."));
            }

            // Verify required propertys
            if(!ModelState.IsValid)
            {
                logger.LogWarning(LoggingEvents.ModelState, "Model State not valid!");
                return BadRequest(new ErrorResponse("[ERROR] - User or password invalid!"));
            }

            // Get in the database the user hash password
            logger.LogInformation(LoggingEvents.GetItem, "GetUser({0})", user.Email);
            var entity = await repository.GetAsync<User>(c => c.Email == user.Email);

            // Verify hashs
            if (entity == null || !encryptor.VerifyPassword(entity.Password, user.Password))
            {
                logger.LogWarning(LoggingEvents.Unauthorized, "Try to login in account: {0}.", user.Email);
                return BadRequest(new ErrorResponse("[ERROR] - User or password invalid!"));
            }

            // Create token
            logger.LogInformation(LoggingEvents.UserLogin, "New Login. Email: {0}", user.Email);
            var encodedToken = tokenProvider.CreateEncoded(entity.FirstName);

            return Ok(new
            {
                User = entity,
                Token = encodedToken,
                Expires = DateTime.Now
            });
        }
    }
}