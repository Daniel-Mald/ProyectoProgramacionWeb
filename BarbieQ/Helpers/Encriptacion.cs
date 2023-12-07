using System.Security.Cryptography;
using System.Text;

namespace BarbieQ.Helpers
{
    public static class Encriptacion
    {
        public static string StringToSHA512(string x)
        {
            using(var sha512 = SHA512.Create())
            {
                var arreglo = Encoding.UTF8.GetBytes(x);
                var hash = sha512.ComputeHash(arreglo);
                return Convert.ToHexString(hash).ToLower();
            }
        }
    }
}
