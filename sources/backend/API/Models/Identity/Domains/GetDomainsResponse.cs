using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademyCloud.API.Models.Identity.Domains
{
    public class GetDomainsResponse
    {
        public IEnumerable<Domain> Domains { get; set; }
    }
}
