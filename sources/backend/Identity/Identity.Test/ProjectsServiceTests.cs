using AcademyCloud.Identity.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcademyCloud.Identity.Protos.Projects;
using AcademyCloud.Identity.Protos;
using AcademyCloud.Identity.Services;
using Xunit;
using static AcademyCloud.Identity.Test.Helpers.AuthenticatedCallContext;

namespace AcademyCloud.Identity.Test
{
    public class ProjectsServiceTests : CommonTest
    {
        protected ProjectsService service;

        public ProjectsServiceTests()
        {
            // run all tests as njuadmin
            service = new ProjectsService(db, MockTokenClaimsAccessor(db, njuadmin).Result);
        }

        [Fact]
        public async Task AddUserToProject()
        {
            Assert.Equal(1, lqproject.Users.Count(x => x.Role == UserRole.Admin));

            await service.AddUserToProject(new AddUserToProjectRequest
            {
                UserId = fgh.Id.ToString(),
                ProjectId = lqproject.Id.ToString(),
                Role = Protos.Common.UserRole.Admin,
            }, TestContext);

            Assert.Equal(2, lqproject.Users.Count(x => x.Role == UserRole.Admin));
            Assert.Equal(3, lqproject.Users.Count());

        }

        [Fact]
        public async Task ChangeUserRole()
        {
            Assert.Equal(1, lqproject.Users.Count(x => x.Role == UserRole.Admin));

            await service.ChangeUserRole(new ChangeUserRoleRequest
            {
                UserId = cjd.Id.ToString(),
                ProjectId = lqproject.Id.ToString(),
                Role = Protos.Common.UserRole.Admin,
            }, TestContext);

            Assert.Equal(2, lqproject.Users.Count(x => x.Role == UserRole.Admin));
        }

        [Fact]
        public async Task CreateProject()
        {
            Assert.Equal(2, db.Projects.Count());

            await service.CreateProject(new CreateProjectRequest
            {
                Name = "test",
                AdminId = cjd.Id.ToString(),
            }, TestContext);

            Assert.Equal(3, db.Projects.Count());

            var project = db.Projects.First(x => x.Name == "test");
            Assert.Single(project.Users);
            Assert.Equal(cjd.Id, project.Users.First().User.Id);
            Assert.Equal(nju, project.Domain);
        }

        [Fact]
        public async Task DeleteProject()
        {
            Assert.Equal(2, db.Projects.Count());

            await service.DeleteProject(new DeleteProjectRequest
            {
                ProjectId = lqproject.Id.ToString()
            }, TestContext);

            Assert.Equal(1, db.Projects.Count());
            Assert.Empty(cjd.Projects);
            Assert.Empty(lq.Projects);
        }

        [Fact]
        public async Task GetAccessibleProjects()
        {
            var resp = await service.GetAccessibleProjects(new GetAccessibleProjectsRequest { }, TestContext);

            Assert.Single(resp.Projects);

            var project = resp.Projects.First();
            Assert.Equal(lqproject.Id.ToString(), project.Id);

            Assert.Single(project.Admins);
            Assert.Equal(lq.Id.ToString(), project.Admins.First().User.Id);

            Assert.Single(project.Members);
            Assert.Equal(cjd.Id.ToString(), project.Members.First().User.Id);
        }

        [Fact]
        public async Task GetUsersOfProject()
        {
            var resp = await service.GetUsersOfProject(new GetUsersOfProjectRequest
            {
                ProjectId = lqproject.Id.ToString(),
            }, TestContext);

            Assert.Single(resp.Admins);
            Assert.Equal(lq.Id.ToString(), resp.Admins.First().User.Id);

            Assert.Single(resp.Members);
            Assert.Equal(cjd.Id.ToString(), resp.Members.First().User.Id);
        }

        [Fact]
        public async Task RemoveUserFromProject()
        {
            await service.RemoveUserFromProject(new RemoveUserFromProjectRequest
            {
                ProjectId = lqproject.Id.ToString(),
                UserId = cjd.Id.ToString()
            }, TestContext);

            Assert.Empty(cjd.Projects);
            Assert.Single(lqproject.Users);


        }

    }
}
