﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class User
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public uint Salary { get; set; }
        public string Email { get; set; }
        public ushort FamilyId { get; set; } = 0;
        public User(string Name, string Password, string email)
        {
            this.Name = Name;
            this.Password = Password;
            Email = email;
        }
    }
}
