using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Account
{
    public class UpdatePasswordRequest
    {
        public string Original { get; set; }

        public string Updated { get; set; }
    }
}
