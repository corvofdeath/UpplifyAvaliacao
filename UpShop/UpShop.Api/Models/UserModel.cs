using MongoDB.Bson;
using UpShop.Api.Utils;
using UpShop.Dominio.Entitys;

namespace UpShop.Api.Models
{
    public class UserModel : IMapperEntity<User>
    {
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // track if password change
        public bool IsPasswordChanged { get; set; } = false;

        // Defines Mapper operation to super entity to this DTO
        public void MapperProperties(User entity)
        {
            if(!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(FirstName))
            {
                entity.FirstName = this.FirstName;
            }

            if(!string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(LastName))
            {
                entity.LastName = this.LastName;
            }

            if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Password))
            {
                entity.Password = this.Password;
                IsPasswordChanged = true;
            }

            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Email))
            {
                entity.Email = this.Email;
            }

            if (!string.IsNullOrEmpty(Phone) && !string.IsNullOrEmpty(Phone))
            {
                entity.Phone = this.Phone;
            }
        }
    }
}
