using AcademyCloud.Expenses.Data;
using AcademyCloud.Expenses.Domain.Entities;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DomainEntity = AcademyCloud.Expenses.Domain.Entities.Domain;
using static AcademyCloud.Shared.Constants;
using AcademyCloud.Expenses.Extensions;
using AcademyCloud.Shared;
using Moq;
using Microsoft.AspNetCore.Http;

namespace AcademyCloud.Expenses.Test.Helpers
{
    public class CommonTest : IDisposable
    {
        protected ExpensesDbContext db;

        public User cjd = new User(Guid.NewGuid(), 10);
        public User cjy = new User(Guid.NewGuid(), 20);
        public User lq = new User(Guid.NewGuid(), 30);
        public User fgh = new User(Guid.NewGuid(), 40);
        public User fc = new User(Guid.NewGuid(), 50);
        public User njuadmin = new User(Guid.NewGuid(), 60);
        public DomainEntity nju;
        public DomainEntity pku;
        public Project lqproject;
        public Project fcproject;
        public UserProjectAssignment lq67project;
        public UserProjectAssignment cjd67project;
        public UserProjectAssignment fcfcproject;

        public TokenClaims cjdlqTokenClaims;
        public TokenClaims lqlqTokenClaims;
        public TokenClaims njuadminnjuTokenClaims;
        public TokenClaims fcfcTokenClaims;

        public void InitializeVariables()
        {
            nju = new DomainEntity(Guid.NewGuid(), njuadmin, new Domain.ValueObjects.Resources(10, 20, 30));
            pku = new DomainEntity(Guid.NewGuid(), fc, new Domain.ValueObjects.Resources(20, 30, 40));
            cjd.Domains.Add(nju);
            cjd.Domains.Add(pku);
            lq.Domains.Add(nju);
            njuadmin.Domains.Add(nju);
            fc.Domains.Add(pku);

            lqproject = new Project(Guid.NewGuid(), lq, nju, new Domain.ValueObjects.Resources(5, 10, 20));
            fcproject = new Project(Guid.NewGuid(), fc, pku, new Domain.ValueObjects.Resources(10, 30, 40));
            lq67project = new UserProjectAssignment(Guid.NewGuid(), lq, lqproject, new Domain.ValueObjects.Resources(4, 8, 10));
            cjd67project = new UserProjectAssignment(Guid.NewGuid(), cjd, lqproject, new Domain.ValueObjects.Resources(1, 2, 10));
            fcfcproject = new UserProjectAssignment(Guid.NewGuid(), fc, fcproject, new Domain.ValueObjects.Resources(5, 15, 20));

            cjdlqTokenClaims = new TokenClaims(false, false, cjd.Id, nju.Id, lqproject.Id, UserRole.Member);
            lqlqTokenClaims = new TokenClaims(false, false, lq.Id, nju.Id, lqproject.Id, UserRole.Admin);
            njuadminnjuTokenClaims = new TokenClaims(false, false, njuadmin.Id, nju.Id, null, UserRole.Admin);
            fcfcTokenClaims = new TokenClaims(false, false, fc.Id, pku.Id, fcproject.Id, UserRole.Admin);
        }

        public void FillData(ExpensesDbContext context)
        {
            // system user: system, system
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            context.Domains.AddRange(nju, pku);

            context.Users.AddRange(cjd, cjy, lq, fgh, fc, njuadmin);

            context.Projects.AddRange(lqproject, fcproject);

            context.UserProjectAssignments.AddRange(lq67project, cjd67project, fcfcproject);

            context.SaveChanges();
        }

        public CommonTest()
        {
            InitializeVariables();
            var options = new DbContextOptionsBuilder<ExpensesDbContext>()
                .UseLazyLoadingProxies()
                .UseSqlite("DataSource=:memory:")
                .Options;

            db = new ExpensesDbContext(options);

            FillData(db);

            db.Entry(db.Users.Find(SocialDomainAdminId)).Collection(x => x.Domains).Load();
        }

        public void Dispose()
        {
            db.Database.EnsureDeleted();
            db.Dispose();
        }

        public ServerCallContext TestContext => TestServerCallContext.Create();

        public TokenClaimsAccessor MockTokenClaimsAccessor(TokenClaims tokenClaims)
        {
            var mockHttpAccessor = new Mock<IHttpContextAccessor>();

            var httpContext = new DefaultHttpContext
            {
                User = tokenClaims.ToClaimsPrincipal()
            };

            mockHttpAccessor.Setup(x => x.HttpContext).Returns(httpContext);

            return new TokenClaimsAccessor(mockHttpAccessor.Object);
        }
    }
}
