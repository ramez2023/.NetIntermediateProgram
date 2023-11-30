/*


Here is an example of a method for generating password hash:
   
   public string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
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
   
Try to review and optimize the code to improve the performance of the method. Do not reduce iterations’ number.

*/

namespace PasswordHashConsoleApp;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        byte[] salt = new byte[16];
        string passwordText = "your_password_here";

        for (int i = 0; i < int.MaxValue; i++)
        {
            Console.WriteLine($"Count {i}");

           //PasswordHashGenerator.GeneratePasswordHashUsingSaltOriginal(passwordText, salt);
             PasswordHashGenerator.GeneratePasswordHashUsingSaltOptimized(passwordText, salt);
        }
    }
}