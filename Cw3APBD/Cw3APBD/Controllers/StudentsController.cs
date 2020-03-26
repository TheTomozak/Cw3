using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private String ConString = "Data Source=db-mssql;Initial Catalog=s18969;Integrated Security=True";

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
        public IActionResult GetStudents()   // przekazywane danych z pomocą QueryString
        {
            // return $"Kowalski, Malewski, Andrzejewski sortowanie={orderBy}";
            // return Ok(_dbService.GetStudents());

            var listStudents = new List<Student>();
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select * from student";

                con.Open();
                SqlDataReader dataReader = com.ExecuteReader();
                             

                while (dataReader.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dataReader["IndexNumber"].ToString();
                    st.FirstName = dataReader["FirstName"].ToString();
                    st.LastName = dataReader["LastName"].ToString();
                    st.BirthDate = DateTime.Parse(dataReader["BirthDate"].ToString());
                    st.IdEnrollment = int.Parse(dataReader["IdEnrollment"].ToString());
                    listStudents.Add(st);
                }

            }



            return Ok(listStudents);

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