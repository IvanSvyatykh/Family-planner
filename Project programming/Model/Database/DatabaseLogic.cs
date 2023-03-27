using Microsoft.EntityFrameworkCore;
using Classes;
using Database;
using Members;



namespace WorkWithDatabase
{
    public class DatabaseLogic
    {
        public static async Task<bool> IsUserExistsAsync(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Users.Where(u => u.Email.Equals(user.Email)).AnyAsync();
            }
        }

        public static async Task<bool> AddSalaryToUserAsync(string UserEmail, uint salary)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    User? user = null;
                    await Task.Run(async () =>
                    {
                        user = await db.Users.Where(u => u.Email.Equals(UserEmail)).FirstOrDefaultAsync();
                        user.Salary = salary;
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

        public static async Task<bool> IsUserPasswordCorrectAsync(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User userFromDb = null;
                await Task.Run(async () =>
                {
                    userFromDb = await db.Users.Where(u => u.Email.Equals(user.Email)).FirstOrDefaultAsync();

                });
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

        public static async Task<bool> ChangeUserPasswordAsync(string UserEmail, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    User? user = null;
                    await Task.Run(async () =>
                    {
                        user = await db.Users.Where(u => u.Email.Equals(UserEmail)).FirstOrDefaultAsync();
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

        public static async Task<User> GetFullPersonInformation(string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {

                try
                {
                    User? user = null;
                    await Task.Run(async () =>
                     {
                         user = await db.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
                     });
                    return user;
                }
                catch
                {
                    return null;
                }
            }

        }

        private static async Task<bool> AddFamilyIdToCreatorAsync(string email, ushort FamilyId)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    User? user = null;
                    await Task.Run(async () =>
                    {
                        user = await db.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
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

        public static async Task<bool> AddFamilyToUserAsync(string CreatorEmail, string Useremail)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    User? user = null;
                    await Task.Run(async () =>
                    {
                        Family family = await db.Families.Where(f => f.Email == CreatorEmail).FirstOrDefaultAsync();
                        user = await db.Users.Where(u => u.Email == Useremail).FirstOrDefaultAsync();
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

        public static async Task<bool> IsExistFamilyAsync(string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return await db.Families.Where(f => f.Email == email).AnyAsync();
            }
        }

        public static Family GetFullFamilyInformation(ushort Id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Family? family = null;
                try
                {
                    family = db.Families.Where(f => f.Id == Id).FirstOrDefault();
                    return family;
                }
                catch
                {
                    family = new Family(null, null, null);
                    return family;
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
                    family = await db.Families.Where(f => f.Email == email).FirstOrDefaultAsync();
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
                    family = await db.Families.Where(f => f.Email == family.Email).FirstOrDefaultAsync();

                    return true && await AddFamilyIdToCreatorAsync(family.Email, family.Id);
                }
                return false;
            }
        }

        public static async Task<ushort?> GetFamilyIdAync(string email)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                ushort? Id = null;
                await Task.Run(async () =>
                {
                    Family family = await db.Families.Where(f => f.Email.Equals(email)).FirstOrDefaultAsync();
                    Id = family.Id;
                });
                return Id;
            }
        }
    }
}