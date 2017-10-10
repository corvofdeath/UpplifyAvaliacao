namespace UpShop.Api.Auth
{
    /// <summary>
    /// Using BCrypt to create and verify hashs
    /// </summary>
    public class Encryptor
    {
        public string GetEncriptedPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashPassword, string inputPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashPassword);
        }

    }
}
