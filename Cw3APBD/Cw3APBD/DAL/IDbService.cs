using Cw3APBD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3APBD.DAL
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}
