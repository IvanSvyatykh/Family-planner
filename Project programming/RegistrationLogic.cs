using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration
{
   public  class RegistrationLogic
    {
        public static bool IsPasswordEquals(string Password , string RepeatedPassword) => Password.Equals(RepeatedPassword);
        public static bool IsFieldsCorrect(params string[] fields)
        {
            for(int i =0;i<fields.Length;i++) 
            {
                if (fields[i]==null) return false;  
            }
            return true;
        }

    }
}
