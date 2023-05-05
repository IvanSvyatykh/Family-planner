using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordLogic
{
    public class PasswordLog
    {
        public static int RandomNumberGenerator()
        {
            Random random = new Random();
            return random.Next(10000, 99999);
        }

        public string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
        }

    }
}
