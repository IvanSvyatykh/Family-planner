using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordLogic
{
    public class PasswordLog
    {
        public static int RandomNumberGenerator()
        {
            Random random= new Random();
            return random.Next(10000,99999);
        }
    }
}
