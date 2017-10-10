using System.Collections.Generic;
using UpShop.Dominio.Helpers;

namespace UpShop.Dominio.Entitys
{
    [Collection("Users")]
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // relations
        public IList<Historic> Historics { get; set; }

        public User()
        {
            Historics = new List<Historic>();
        }
    }
}
