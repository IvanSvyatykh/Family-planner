using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasswordLogic;
using Classes;
using Project_programming.Model.Database;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Xml.Linq;


namespace WorkWithDatabase
{
    public class DatabaseLogic
    {
        public static async Task<bool> IsUserExistsAsync(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Users.Where(u => u.Email == user.Email).AnyAsync();
            }
        }

        public static async Task<bool> IsPasswordCorrectAsync(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                User userFromDb = db.Users.Where(u => u.Email == user.Email).FirstOrDefault();
                return userFromDb.Password.Equals(user.Password);
            }
        }

        public static async Task<bool> AddUserAsync(string name, string password, string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = new User(name, password, email);
                if (!await IsUserExistsAsync(user))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static async Task<bool> ChangePasswordAsync(string email, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    User? user = null;
                    await Task.Run(() =>
                    {
                        user = db.Users.Where(u => u.Email == email).FirstOrDefault();
                        user.Password = password;
                        db.SaveChanges();
                    });

                    return true;
                }
                catch
                {
                    return false;
                }

            }

        }

        public static async Task<bool> IsExistFamilyAsync(string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Families.Where(f => f.Email == email).AnyAsync();
            }
        }

        public static User GetFullPersonInformation(string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                try
                {
                    User? user = null;
                    user = db.Users.Where(u => u.Email == email).FirstOrDefault();
                    return user;
                }
                catch
                {
                    return null;
                }
            }

        }

        public static async Task<Family> GetFullFamilyInformationAsync(ushort Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {

                    Family? family = null;
                    await Task.Run(() =>
                    {
                        family = db.Families.Where(f => f.Id == Id).FirstOrDefault();

                    });
                    return family;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static async Task<bool> AddFamilyToUserAsync(string CreatorEmail, string Useremail)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    User? user = null;
                    await Task.Run(() =>
                    {
                        Family family = db.Families.Where(f => f.Email == CreatorEmail).FirstOrDefault();
                        user = db.Users.Where(u => u.Email == Useremail).FirstOrDefault();
                        user.FamilyId = family.Id;
                        db.SaveChanges();
                    });

                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }
        private static async Task<bool> AddFamilyIdToCreator(string email, uint FamilyId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    User? user = null;
                    await Task.Run(() =>
                    {
                        user = db.Users.Where(u => u.Email == email).FirstOrDefault();
                        user.FamilyId = FamilyId;
                        db.SaveChanges();
                    });
                    return true;
                }
                catch
                {
                    return false;
                }

            }
        }

        public static async Task<bool> IsFamilyPasswordCorrectAsync(string email, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Family? family = null;
                await Task.Run(async () =>
                {
                    family = db.Families.Where(f => f.Email == email).FirstOrDefault();
                });
                return family.Password.Equals(password);
            }

        }

        
        public static async Task<bool> CreateFamilyAsync(Family family, User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (!await IsExistFamilyAsync(family.Email))
                {
                    db.Families.Add(family);
                    db.SaveChanges();
                    family = db.Families.Where(f => f.Email == family.Email).FirstOrDefault();

                    return true && await AddFamilyIdToCreator(family.Email, family.Id);
                }
                return false;
            }
        }


    }
}

