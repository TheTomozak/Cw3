using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw3APBD.DAL;
using Cw3APBD.DTOs.Requests;
using Cw3APBD.Models;
using Cw3APBD.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw3APBD.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentsDbService _server;

        public EnrollmentsController(IStudentsDbService service)
        {
            _server = service;
        }

        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            _server.EnrollStudent(request);

            return Ok(request);
        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudents(EnrollPromotionsRequest enrollPromotionsRequest)
        {
            _server.PromoteStudent(enrollPromotionsRequest);

            return Ok(enrollPromotionsRequest);
        }
    }
}