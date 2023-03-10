using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;
using Database;
using Microsoft.EntityFrameworkCore;

namespace WorkWithDatabase
{
    public class DatabaseLogic
    {
        private static bool IsExists(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = db.Users.ToList();
               
                return true;
            }
        }
        public static bool AddUser(string name, string password, string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = new User(name, password, email);
                if (IsExists(user))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
