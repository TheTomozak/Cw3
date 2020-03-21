using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3APBD.DAL;
using Cw3APBD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3APBD.Controllers
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private readonly IDbService _dbService;
        
        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }
        /*
        [HttpGet]
        public string GetStudents()
        {
            return "Kowalski, Malewski, Andrzejewski";
        }
        */

        [HttpGet]
        public IActionResult GetStudents(string orderBy)   // przekazywane danych z pomocą QueryString
        {
            // return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
            return Ok(_dbService.GetStudents());
        }

        


        [HttpGet("{id}")]
        public IActionResult GetStudents(int id)
        {

            if (id == 1)
            {
                return Ok("Kowalski");
            }
            else if (id == 2)
            {
                return Ok("Malewski");
            }
           

            return NotFound("Nie znaleziono studenta");

        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            // add to database 
            // generating index number

            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }


        [HttpPut("{id}")]
        public IActionResult EditStudent( int id)
        {
            return Ok("Update succesful");
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {

            return Ok("Delete successful");
        }

    }
}