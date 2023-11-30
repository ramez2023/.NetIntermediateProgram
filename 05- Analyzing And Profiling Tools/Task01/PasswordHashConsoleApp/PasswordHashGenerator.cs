using System.Security.Cryptography;

namespace PasswordHashConsoleApp;
internal class PasswordHashGenerator
{
    /*

    After combining salt and hash arrays into combinedBytes using Buffer.BlockCopy, the salt and hash arrays essentially don't exist in memory.
    The combinedBytes array contains the concatenated data of both salt and hash.

    In the original code, even after creating hashBytes, you still have the salt and hash arrays in memory until they are garbage collected by the runtime.
    The optimized code avoids creating an additional array (hashBytes) by directly merging the data into combinedBytes.
    This can be more memory-efficient, especially when dealing with large datasets, as it reduces the number of arrays created and memory copied.

    */

    public static string GeneratePasswordHashUsingSaltOriginal(string passwordText, byte[] salt)
    {

        var iterate = 10000;
        var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
        byte[] hash = pbkdf2.GetBytes(20);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        var passwordHash = Convert.ToBase64String(hashBytes);

        return passwordHash;
    }

    public static string GeneratePasswordHashUsingSaltOptimized(string passwordText, byte[] salt)
    {
        const int iterate = 10000;
        const int derivedKeyLength = 20;
        const int saltLength = 16;

        using (var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate))
        {
            byte[] hash = pbkdf2.GetBytes(derivedKeyLength);

            byte[] hashBytes = new byte[saltLength + derivedKeyLength];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, saltLength);
            Buffer.BlockCopy(hash, 0, hashBytes, saltLength, derivedKeyLength);

            return Convert.ToBase64String(hashBytes);
        }

    }

}
