using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Authentication;

namespace AcademyCloud.API.Models
{
    public class ChangeScopeRequest
    {
        public Scope Scope { get; set; }
    }
}
