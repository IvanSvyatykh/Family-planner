using MySQLApp;

namespace Project_programming.Database
{
    public class WorkWithDatabase
    {
        public static void Try()
        {
            // добавление данных
            using (ApplicationContext db = new ApplicationContext())
            {
                User user1 = new User { FirstName = "Tom", Salary = 33 , Id=1 , _email="vvrvrvrv"  };
                User user2 = new User { FirstName = "Alice", Salary = 26, Id = 2, _email = "81912" };

                db.Users.AddRange(user1, user2);
                db.SaveChanges();
            }
            // получение данных
            using (ApplicationContext db = new ApplicationContext())
            {
                var users = db.Users.ToList();
               
            }
        }
    }
}
