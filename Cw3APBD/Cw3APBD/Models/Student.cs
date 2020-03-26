using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3APBD.Models
{
    public class Student
    {

        public int IdStudent { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string IndexNumber { get; set; }

        public int IdEnrollment { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
