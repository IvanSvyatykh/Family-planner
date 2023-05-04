using Classes;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace Database
{
    public class SQLGoodsCategoriesRepository
    {
        private readonly ApplicationContext db;

        public SQLGoodsCategoriesRepository()
        {
            db = new ApplicationContext();
        }
        public List<GoodsCategory> GetAllUsersCategories(uint userId)
        {
            List<GoodsCategory> goods =  db.Goods.Where(g => g.UserId == userId).ToList();
            goods.Sort((l, r) => l.Name.CompareTo(r.Name));
            return goods;
        }

        public List<string> GetAllUsersCategoriesName(uint userId)
        {
            List<string> names =  db.Goods.Where(g => g.UserId == userId).Select(g => new string(g.Name)).ToList();
            names.Sort((l, r) => l.CompareTo(r));
            return names;
        }

        public async Task<GoodsCategory> AddCategoryAsync(string categoryName, uint userId)
        {
            await db.Goods.AddAsync(new GoodsCategory { Name = categoryName, UserId = userId });
            await db.SaveChangesAsync();
            return await db.Goods.Where(g => g.Name.Equals(categoryName)).FirstOrDefaultAsync();
        }

        public async void AddUniqueCategoryAsync(string categoryName, uint userId)
        {
            await db.Goods.AddAsync(new GoodsCategory { Name = categoryName, UserId = userId });
            await db.SaveChangesAsync();
        }
        public async Task<bool> RemoveCategory(GoodsCategory goodsCategory)
        {
            try
            {
                await Task.Run(async () =>
                {
                    db.Goods.Remove(goodsCategory);
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
