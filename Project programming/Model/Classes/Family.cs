using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;

namespace Classes
{
    public class Family
    {
        public ushort Id { get; private set; }
        public string Name { get; private set; } = null!;
        public uint? Balance { get; private set; }
        public string Password { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public Family(string Name, string Password, string email)
        {
            this.Name = Name;
            this.Password = Password;
            Email = email;
        }       
    }
}
