﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;
using Database;
using Microsoft.EntityFrameworkCore;
using PasswordLogic;

namespace WorkWithDatabase
{
    public class DatabaseLogic
    {
        public static async Task<bool> IsExistsAsync(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Users.Where(u => u.Email == user.Email).AnyAsync();
            }
        }

        public static async Task<bool> IsPasswordCorrect(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Users.Where(u => u.Password.Equals(user.Password)).AnyAsync();
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

        public static async Task<bool> ChangePassword(string email, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    User? user = null;
                    user = db.Users.Where(u => u.Email == email).FirstOrDefault();
                    user.Password = password;
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }

            }

        }
    }
}
