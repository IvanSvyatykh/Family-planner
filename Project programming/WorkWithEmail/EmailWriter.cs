using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Project_programming.WorkWithEmail
{
    public static class EmailWriter
    {
        private static string Password = "MTH5NJzZgtuuWARxUVpc";// пароль отправителя
        private static string Email = "family.planner@mail.ru";// адреса отправителя      
        public static bool SendMessage(string adressTo, string messageSubject, string messageText)
        {
            try
            {
                //пытаемся отправить сообщение по указаной почте
                MailMessage mess = new MailMessage();
                mess.To.Add(adressTo); // адрес получателя
                mess.From = new MailAddress(Email);
                mess.Subject = messageSubject; // тема
                mess.Body = messageText; // текст сообщения
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.mail.ru"; //smtp-сервер отправителя
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(Email, Password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mess); // отправка пользователю              
                mess.Dispose();
                return true;
            }
            catch (Exception e)
            {
                //на случай если пользователь ошибся в почте и отправка не возможна
                return false;
            }
        }

    }
}
