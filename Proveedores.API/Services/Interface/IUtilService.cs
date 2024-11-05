namespace Proveedores.API.Services
{
    public interface IUtilService
    {
       public bool IsValidEmail(string email);
       public bool IsValidPassword(string password);

       public string HashPassword(string password, byte[]? salt = null, bool needsOnlyHash = false);

       public bool VerifyPassword(string hashedPasswordWithSalt, string passwordToCheck);
    }
}