using Classes;

namespace DataContext
{
    public static class CurrentDataContext
    {
        private static User _user;
        private static Family _family;

        public static void AddUser(User user)
        {
            _user = user;
        }
        public static void AddFamily(Family family)
        {
            _family = family;
        }
       
        public static void ChangeSalary(uint salary)
        {
            _user.Salary = salary;
        }
        public static void RemoveFamily()
        {
            _family = null;
        }
        public static void AddFamilyIdToUser(ushort FamilyId)
        {
            _user.FamilyId = FamilyId;
        }
        public static Family GetFamily => _family;
        public static User GetUser => _user;
        public static string GetUserEmail => _user.Email;
        public static string GetUserName => _user.Name;
        public static string GetUserSalary=>_user.Salary.ToString();
        public static string GetFamilyEmail=>_family.Email;
        public static ushort GetUserFamailyId => _user.FamilyId;

    }
}
