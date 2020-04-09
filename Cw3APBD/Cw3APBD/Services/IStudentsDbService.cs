using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3APBD.DTOs;
using Cw3APBD.DTOs.Requests;
using Cw3APBD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cw3APBD.Services
{
    public interface IStudentsDbService
    {
        EnrollExceptionHelper EnrollStudent(EnrollStudentRequest request);
        void PromoteStudent(EnrollPromotionsRequest request);

        Student GetStudent(string indexNumber);
    }
}
