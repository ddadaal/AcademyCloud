using AcademyCloud.Identity.Data;
using AcademyCloud.Identity.Services.Domains;
using Grpc.Core;
using Identity.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Identity.Test.Helpers.MockDbContext;

namespace Identity.Test
{
    public class CommonTest: IDisposable
    {
        protected IdentityDbContext db;
        protected DomainsService service;

        public CommonTest()
        {
            db = GetDbContext();
            service = new DomainsService(db);
        }

        public void Dispose()
        {
            db.Database.EnsureDeleted();
            db.Dispose();
            service = null!;
        }
        
        public async Task<ServerCallContext> GetContext(string username = "system", string password = "system")
        {
            return await AuthenticatedCallContext.Create(db, username, password);


        }
    }
}
