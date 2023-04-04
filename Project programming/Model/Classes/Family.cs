﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;

namespace Classes
{
    public class Family
    {
        public ushort Id { get; set; }
        public string Name { get; set; } = null!;
        public uint? Balance { get; set; }
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public Family(string Name, string Password, string email)
        {
            this.Name = Name;
            this.Password = Password;
            Email = email;
        }
    }
}