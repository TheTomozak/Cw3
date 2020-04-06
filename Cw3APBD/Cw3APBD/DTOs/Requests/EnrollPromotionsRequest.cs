using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3APBD.DTOs.Requests
{
    public class EnrollPromotionsRequest
    {
        [Required(ErrorMessage = "You must give name studies")]
        [MaxLength(250)]
        public string Studies { get; set; }
        [Required(ErrorMessage = "You must give ")]
        [Range(1,10)]
        public int Semester { get; set; }
    }
}
