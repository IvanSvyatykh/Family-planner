using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Project_programming.WorkWithEmail
{
    public class CheckEmailCorectness
    {
        public static bool IsValidEmail(string Email)
        {
            try
            {
                MailAddress mail = new MailAddress(Email);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
