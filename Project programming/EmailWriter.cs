using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithEmail
{
    public static class EmailWriter
    {
        private static string Password = "";// пароль отправителя
        private static string Email = "family.planner@mail.ru";// адреса отправителя      
        public static void SendMessage(string adressTo, string messageSubject, string messageText)
        {
            try
            {                        
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
                return;
            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }

    }
}
