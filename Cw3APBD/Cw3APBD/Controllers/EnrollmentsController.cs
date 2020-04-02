using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw3APBD.DAL;
using Cw3APBD.DTOs.Requests;
using Cw3APBD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw3APBD.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private string ConString = "Data Source=db-mssql;Initial Catalog=s18969;Integrated Security=True";

       

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student();
            st.FirstName = request.FirstName;
            st.LastName = request.LastName;
            st.BirthDate = request.BirthDate;
            st.Studies = request.Studies;
            st.IndexNumber = request.IndexNumber;

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                var transaction = con.BeginTransaction();

                try
                {
                    com.CommandText = "SELECT IdStudy FROM Studies WHERE name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        transaction.Rollback();
                        return BadRequest("Studies not exist");
                    }

                    int idStudy = (int) dr["IdStudy"];


                    com.CommandText = "SELECT IdEnrollment FROM Enrollment WHERE IdStudy = @idStudy AND Semester=1";
                    com.Parameters.AddWithValue("idStudy", idStudy);

                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        com.CommandText = "INSERT INTO Enrollment (Semester, IdStudy, StartDate)" +
                                          "VALUES (1, @idStudy, @dateNow)";
                        com.Parameters.AddWithValue("dateNow", DateTime.Now);
                    }

                    dr = com.ExecuteReader();

                    com.CommandText = "SELECT 'X' FROM Student WHERE indexNumber=@indexNumber ";
                    com.Parameters.AddWithValue("IndexNumber", request.IndexNumber);

                    if (dr.Read())
                    {
                        transaction.Rollback();
                        return BadRequest($"Student with {request.IndexNumber} exist");
                    }

                    int idEnrollment = (int) dr["IdEnrollment"];
                    dr = com.ExecuteReader();


                    com.CommandText =
                        "INSERT INTO Students(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUE (@indexNumber, @firstName, @lastName, @birthDate, @idEnrollment) ";
                    com.Parameters.AddWithValue("indexNumber", request.IndexNumber);
                    com.Parameters.AddWithValue("firstName", request.FirstName);
                    com.Parameters.AddWithValue("lastName", request.LastName);
                    com.Parameters.AddWithValue("birthDate", request.BirthDate);
                    com.Parameters.AddWithValue("idEnrollment", idEnrollment);

                    com.ExecuteNonQuery();
                    transaction.Commit();

                }
                catch (SqlException)
                {
                    transaction.Rollback();
                }
            }


            return Ok(request);
        }
    }
}