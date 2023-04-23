using Classes;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class SQLUserRepository
    {
        private readonly ApplicationContext db;
        public SQLUserRepository()
        {
            db = new ApplicationContext();
        }
        public async Task<bool> IsUserExistsAsync(string email) => await db.Users.Where(u => u.Email.Equals(email)).AnyAsync();
        public async Task<bool> IsUserPasswordCorrectAsync(User user)
        {
            User userFromDb = null;
            await Task.Run(async () =>
            {
                userFromDb = await db.Users.Where(u => u.Email.Equals(user.Email)).FirstOrDefaultAsync();

            });
            return userFromDb.Password.Equals(user.Password);
        }
        public async Task<bool> AddUserAsync(User user)
        {
            if (!await IsUserExistsAsync(user.Email))
            {
                await Task.Run(async () =>
                {
                    await db.Users.AddAsync(user);
                    await db.SaveChangesAsync();
                });
                return true;
            }
            return false;
        }
        public async Task<bool> ChangeUserPasswordAsync(string UserEmail, string password)
        {
            try
            {
                User user = null;
                await Task.Run(async () =>
                {
                    user = await db.Users.Where(u => u.Email.Equals(UserEmail)).FirstOrDefaultAsync();
                    user.ChangePassword(password);
                    await db.SaveChangesAsync();
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<User> GetFullPersonInformationAsync(string email)
        {
            try
            {
                User user = null;
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
        public async Task<bool> AddFamilyToUserAsync(string CreatorEmail, string UserEmail)
        {
            try
            {
                User user = null;
                await Task.Run(async () =>
                {
                    Family family = await db.Families.Where(f => f.Email == CreatorEmail).FirstOrDefaultAsync();
                    user = await db.Users.Where(u => u.Email == UserEmail).FirstOrDefaultAsync();
                    user.ChangeFamilyId(family.Id);
                    await db.SaveChangesAsync();
                });

                return true;
            }
            catch
            {
                return false;
            }


        }
        public async Task<bool> RemoveMemberOfFamilyAsync(ushort FamilyId)
        {
            try
            {
                await Task.Run(async () =>
                {

                    var userFromDb = await db.Users.FirstOrDefaultAsync(u => u.FamilyId == FamilyId);
                    userFromDb.ChangeFamilyId(0);
                    await db.SaveChangesAsync();
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<User> GetAllAccountWithFamilyId(ushort? FamilyId)
        {

            if (FamilyId != 0)
            {
                List<User> users = new List<User>();

                var accounts = db.Users.Where(u => u.FamilyId == FamilyId);
                foreach (var account in accounts)
                {
                    users.Add(account);
                }
                return users;
            }
            else
            {
                return null;
            }


        }
    }
}
