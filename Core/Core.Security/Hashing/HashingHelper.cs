using System.Security.Cryptography;
using System.Text;

namespace Core.Security.Hashing
{
    // HashingHelper sınıfı
    public static class HashingHelper
    {
        // Verilen parola için bir hash ve salt oluşturan metot
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // HMACSHA512 kullanarak bir hash oluşturuluyor
            using HMACSHA512 hmac = new HMACSHA512();

            // Salt oluşturuluyor
            passwordSalt = hmac.Key;

            // Parola UTF-8 olarak kodlanıp hash'e dönüştürülüyor
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        // Verilen parolanın doğruluğunu kontrol eden metot
        public static bool VerifyPassswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // HMACSHA512 kullanarak bir hash oluşturuluyor ve salt ile başlatılıyor
            using HMACSHA512 hmac = new HMACSHA512(passwordSalt);

            // Verilen parolanın hash değeri hesaplanıyor
            byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Hesaplanan hash değeri ile verilen hash değeri karşılaştırılıyor
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
