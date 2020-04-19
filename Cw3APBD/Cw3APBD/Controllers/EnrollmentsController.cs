using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Cw3APBD.DAL;
using Cw3APBD.DTOs;
using Cw3APBD.DTOs.Requests;
using Cw3APBD.Models;
using Cw3APBD.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Cw3APBD.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _server;

        public IConfiguration Configuration { get; set; }
        public EnrollmentsController(IStudentsDbService service, IConfiguration configuration)
        {
            _server = service;
            Configuration = configuration;

        }
        



        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            EnrollExceptionHelper enrollExceptionHelper = _server.EnrollStudent(request);

            switch (enrollExceptionHelper.Number)
            {
                case 1:
                    return BadRequest("Studies not exist");
                case 2:
                    return BadRequest($"Student with {request.IndexNumber} exist");
                default:
                    return Ok(request);
            }
        }

        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudents(EnrollPromotionsRequest enrollPromotionsRequest)
        {
            _server.PromoteStudent(enrollPromotionsRequest);

            return Ok(enrollPromotionsRequest);
        }

        [HttpGet]
        public IActionResult GetStudent(EnrollPromotionsRequest enrollPromotionsRequest)
        {
            _server.PromoteStudent(enrollPromotionsRequest);

            return Ok(enrollPromotionsRequest);
        }



        [HttpPost]
        public IActionResult Login(LoginRequest request)
        {

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18969;Integrated Security=True"))
            using (var com = new SqlCommand())
            {

                var login = new Student();
                login.IndexNumber = request.Login;
                login.Password = request.Password;
                login.Role = request.Role;

                // com.Connection = con;
                // com.CommandText = "select * from student";
                //
                // con.Open();
                // SqlDataReader dataReader = com.ExecuteReader();


               

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, request.Login),
                    new Claim(ClaimTypes.Name, request.Password),
                    new Claim(ClaimTypes.Role, request.Role),
                   
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken
                (
                    issuer: request.Login,
                    audience: request.Role,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken = Guid.NewGuid()
                });
            }

        }
    }
}