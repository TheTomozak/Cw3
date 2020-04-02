using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3APBD.DTOs.Requests
{
    public class EnrollStudentRequest
    {

        [Required(ErrorMessage = "You must give first name")]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "You must give last name")]
        [MaxLength(200)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "You must give name")]
        [RegularExpression("^s[0-9]+$")]
        public string IndexNumber { get; set; }
        [Required(ErrorMessage = "You must give name studies")]
        [MaxLength(250)]
        public string Studies { get; set; }
        [Required(ErrorMessage = "You must give birth date")]
        public DateTime BirthDate { get; set; }

    }
}
