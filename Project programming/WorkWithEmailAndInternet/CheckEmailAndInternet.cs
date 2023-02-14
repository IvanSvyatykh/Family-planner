using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                // Проверяем на то, что адрес хотя бы теоритически почта
                MailAddress mail = new MailAddress(Email);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
