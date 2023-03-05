using Project_programming;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration
{
    public partial class RegistrationLogic : RegistrationPage
    {
        public static bool IsPasswordEquals(string Password, string RepeatedPassword) => Password.Equals(RepeatedPassword);
        public static bool IsFieldsCorrect(params string[] fields)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] == null) return false;
            }
            return true;
        }      
    }
}
