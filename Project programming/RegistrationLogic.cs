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
        public static bool IsFieldsCorrect(string Password, string RepeatedPassword , string Email , string UserName)
        {
            return Password == null || RepeatedPassword == null || Email == null || UserName == null
           || Password.Length != 5 || RepeatedPassword.Length != 5;
        }

    }
}
