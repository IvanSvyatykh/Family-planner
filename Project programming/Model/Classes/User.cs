using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class User
    {
        public ushort Id { get; set; }
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public uint Salary { get; set; }
        public string Email { get; set; } = null!;
        public ushort FamilyId { get; set; } = 0;
        public User(string Name, string Password, string Email)
        {
            this.Name = Name;
            this.Password = Password;
            this.Email = Email;
        }       

        public void ChangeSalary(uint salary) 
        {
            Salary = salary;
        }
        public void ChangePassword(string password)
        {
            Password = password;
        }

        public void ChangeFamilyId(ushort familyId)
        {
            FamilyId = familyId;
        }



    }
}
