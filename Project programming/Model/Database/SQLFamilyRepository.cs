using Classes;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class SQLFamilyRepository
    {
        private readonly ApplicationContext db;
        public SQLFamilyRepository()
        {
            db = new ApplicationContext();
        }

        public async Task<bool> IsExistFamilyAsync(string email) => await db.Families.Where(f => f.Email == email).AnyAsync();
        public async Task<bool> IsFamilyPasswordCorrectAsync(string email, string password)
        {
            Family family = null;
            await Task.Run(async () =>
            {
                family = await db.Families.Where(f => f.Email == email).FirstOrDefaultAsync();
            });
            return family.Password.Equals(password);
        }
        public async Task<bool> CreateFamilyAsync(Family family)
        {

            if (!await IsExistFamilyAsync(family.Email))
            {
                await db.Families.AddAsync(family);
                await db.SaveChangesAsync();
                return true;
            }
            return false;

        }
        public async Task<ushort> GetFamilyIdAsync(string email)
        {
            ushort Id = 0;
            await Task.Run(async () =>
            {
                Family family = await db.Families.Where(f => f.Email.Equals(email)).FirstOrDefaultAsync();
                Id = family.Id;
            });
            return Id;

        }
        public async Task<Family> GetFullFamilyInformationAsync(ushort Id)
        {
            Family family = new Family(null, null, null);
            await Task.Run(async () =>
            {
                family = await db.Families.Where(f => f.Id == Id).FirstOrDefaultAsync();
            });
            return family;
        }
        public async Task<bool> RemoveFamily(ushort Id)
        {
            try
            {
                await Task.Run(async () =>
                {
                    Family family = await db.Families.Where(f => f.Id == Id).FirstOrDefaultAsync();
                    db.Families.Remove(family);
                    await db.SaveChangesAsync();
                });

                return true;
            }
            catch
            {
                return false;
            }
            
        }

    }
}
