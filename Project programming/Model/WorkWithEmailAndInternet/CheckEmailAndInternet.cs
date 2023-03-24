using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithEmail
{
    public class CheckEmailCorectness
    {
        public static bool IsValidEmail(string Email)
        {
            EmailAddressAttribute emailAddressAttribute = new EmailAddressAttribute();

            return Email!=null && emailAddressAttribute.IsValid(Email);
        }

        public static bool ConnectionAvailable()
        {
            string strServer = "http://www.google.com";
            try
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(strServer);
                HttpWebResponse httpWeb = (HttpWebResponse)httpReq.GetResponse();

                if (HttpStatusCode.OK == httpWeb.StatusCode)
                {
                    // HTTP = 200 - Интернет безусловно есть!
                    httpWeb.Close();
                    return true;
                }
                else
                {
                    // сервер вернул отрицательный ответ, возможно что инета нет
                    httpWeb.Close();
                    return false;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}
