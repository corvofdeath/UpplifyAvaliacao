using System.ComponentModel.DataAnnotations;

namespace UpShop.Api.Models
{
    public class CredentialsModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
