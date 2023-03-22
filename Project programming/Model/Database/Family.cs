using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;

namespace Families
{
    public class Family
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public ushort? Balance { get; set; }
        public ushort Password { get; set; }
        public string CreatorEmail { get; set; }

        public Family(string Name, ushort Password, string _email)
        {
            this.Name = Name;
            this.Password = Password;
            this.CreatorEmail = _email;
        }
    }
}
