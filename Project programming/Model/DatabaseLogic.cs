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
        public static async Task<bool> IsExistsAsync(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Users.Where(u=>u.Email==user.Email).AnyAsync();
            }
        }

        public static async Task<bool> IsPasswordCorrect(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Users.Where(u => u.Password == user.Password).AnyAsync();
            }
        }

        public static async Task<bool> AddUserAsync(string name, string password, string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = new User(name, password, email);
                if (!await IsExistsAsync(user))
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
