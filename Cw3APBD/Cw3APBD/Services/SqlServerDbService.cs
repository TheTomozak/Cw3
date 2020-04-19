using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cw3APBD.DTOs;
using Cw3APBD.DTOs.Requests;
using Cw3APBD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cw3APBD.Services
{
    public class SqlServerDbService : IStudentsDbService
    {
        private string ConString = "Data Source=db-mssql;Initial Catalog=s18969;Integrated Security=True";
       

        public EnrollExceptionHelper EnrollStudent(EnrollStudentRequest request)
        {
            var st = new Student();
            st.FirstName = request.FirstName;
            st.LastName = request.LastName;
            st.BirthDate = request.BirthDate;
            st.Studies = request.Studies;
            st.IndexNumber = request.IndexNumber;
            EnrollExceptionHelper enrollExceptionHelper = new EnrollExceptionHelper();

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                var transaction = con.BeginTransaction();
                com.Transaction = transaction;

                try
                {
                    com.CommandText = "SELECT IdStudy FROM Studies WHERE name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        transaction.Rollback();
                        // return BadRequest("Studies not exist");
                        enrollExceptionHelper.Number = 1;
                        return enrollExceptionHelper;
                    }

                    int idStudy = (int) dr["IdStudy"];

                    dr.Close();
                    com.CommandText =
                        "SELECT IdEnrollment FROM Enrollment WHERE IdStudy = @idStudy AND Semester=1 AND StartDate = (SELECT MAX(StartDate) FROM Enrollment)";
                    com.Parameters.AddWithValue("idStudy", idStudy);

                    dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        com.CommandText = "INSERT INTO Enrollment (Semester, IdStudy, StartDate)" +
                                          "VALUES (1, @idStudy, @dateNow)";
                        com.Parameters.AddWithValue("dateNow", DateTime.Now);
                        dr = com.ExecuteReader();
                    }


                    dr.Close();
                    com.CommandText = "SELECT 'X' FROM Student WHERE indexNumber=@indexNumber";
                    com.Parameters.AddWithValue("indexNumber", request.IndexNumber);
                    dr = com.ExecuteReader();

                    if (!dr.Read())
                    {
                        dr.Close();
                        transaction.Rollback();
                        //return BadRequest($"Student with {request.IndexNumber} exist");
                        enrollExceptionHelper.Number = 2;
                        return enrollExceptionHelper;
                    }

                    int idEnrollment = (int) dr["IdEnrollment"];
                    dr.Close();


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

            return enrollExceptionHelper;
        }

        
        public void PromoteStudent(EnrollPromotionsRequest enrollPromotionsRequest)
        {
            var st = new Student();
            st.Studies = enrollPromotionsRequest.Studies;
            st.Semester = enrollPromotionsRequest.Semester;

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                var transaction = con.BeginTransaction();
                com.Transaction = transaction;


                try
                {
                    com.CommandText = "EXEC @studies, @semester";
                    com.Parameters.AddWithValue("studies", enrollPromotionsRequest.Studies);
                    com.Parameters.AddWithValue("semester", enrollPromotionsRequest.Semester);
                    com.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException)
                {
                    transaction.Rollback();
                }
            }
        }

        public Student GetStudent(string indexNumber)
        {
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                var st = new Student();
                com.Connection = con;
                con.Open();


                com.CommandText = "SELECT * FROM Student WHERE indexNumber=@index";
                com.Parameters.AddWithValue("index", indexNumber);

                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return null;
                }

                st.FirstName = dr["FirstName"].ToString();
                st.LastName = dr["LastName"].ToString();
                st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                st.IdEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                st.IndexNumber = dr["IndexNumber"].ToString();

                return st;
            }
        }

      
    }
}