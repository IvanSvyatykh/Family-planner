using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class User
    {
        public uint Id {get; private set; }
        public string Name { get; private set; } = null!;
        public string Password { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public ushort FamilyId { get; private set; } = 0;
        public User(string Name, string Password, string Email)
        {
            this.Name = Name;
            this.Password = Password;
            this.Email = Email;
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
