using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using UpShop.Dominio.Entitys;
using UpShop.Dominio.Interfaces;

namespace UpShop.Api.Auth
{
    /// <summary>
    /// Handle the auth throught the midleware patterns.
    /// </summary>
    public class AuthenticationHandler : JwtBearerEvents
    {
        /// <summary>
        /// Called every time that a request arrives in a method with '[Authorize]' decorator. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task TokenValidated(TokenValidatedContext context)
        {
            // Get the name associated with the token.
            var userName = context.Ticket.Principal.Identity.Name;
            // Get the global repository.
            var repository = context.HttpContext.RequestServices.GetService(typeof(IRepository)) as IRepository;

            // if NOT exist a register in db corresponding to the identity name of the token.
            if (!repository.Exist<User>(c => c.FirstName == userName))
            {
                // then return Unauthorized.
                context.Response.StatusCode = 401;
                context.SkipToNextMiddleware();
            }

            // Auth successful.
            return Task.FromResult(0);
        }
    }

}
