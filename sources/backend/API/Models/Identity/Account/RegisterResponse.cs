﻿using AcademyCloud.Identity.Protos;
using AcademyCloud.Identity.Protos.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Account
{
    public class RegisterResponse
    {
        public string Token { get; set; }

        public Scope Scope { get; set; }
    }
}
