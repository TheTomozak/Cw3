﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw3APBD.DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Cw3APBD.Services
{
    public interface IStudentsDbService
    {
        void EnrollStudent(EnrollStudentRequest request);
        void PromoteStudent(EnrollPromotionsRequest request);
    }
}