using System.Security.Cryptography;
using System.Text;

namespace mycrypt.Servicios
{
    public static class Seguridad
    {
        public static void CrearPasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public static bool VerificarPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var hashCalculado = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hash.SequenceEqual(hashCalculado);
        }
    }
}
