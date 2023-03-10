using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class User
    {
        
         public int _id { get; set; }

        public string _name { get; set; } = null!;

        public string _password { get; set; }

        public int _salary { get; set; }

        public string _email { get; set; }
    }



}
