using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

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
        public bool IsAdmin { get; set; }
    }



}
