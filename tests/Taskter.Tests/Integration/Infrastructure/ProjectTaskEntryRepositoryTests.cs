using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Taskter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Taskter.Infrastructure.Repositories;
using Taskter.Core.Entities;
using System.Linq;
using Taskter.Infrastructure.UserContext;

namespace Taskter.Tests.Integration.Api
{
    [TestFixture]
    public class ProjectEntryRepositoryTests
    {
        private ProjectTaskEntryRepository _repository;
        private TaskterDbContext _context;
        private ICurrentUserContext _userContext;

        [SetUp]
        public void SetUp()
        {
            _context = new TaskterDbContext(new DbContextOptionsBuilder<TaskterDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);
            _context.Database.EnsureCreated();
            _userContext = new FakeCurrentUserContext() { UserId = Guid.NewGuid() };
            _repository = new ProjectTaskEntryRepository(_context, _userContext);
        }

        [Test]
        public async Task GetProjectTaskEntriesByDate_AddedTimeEntryForGivenDate_ReturnNonEmptyResult()
        {
            this._context.Clients.Add(new Client("testclient1"){Id = 1});
            this._context.Projects.Add(new Project("testProject1", 1, "examplecode"){Id = 1});
            this._context.ProjectTasks.Add(new ProjectTask("testTask1", 1, true){Id = 1});
            _context.SaveChanges();
            var newEntry = new ProjectTaskEntry(_userContext.UserId, 1, 50, new DateTime(2019, 2, 10), "Nasa nota"){Id = 50};

            await _repository.AddTimeEntry(newEntry);
            var result = await _repository.GetProjectTaskEntriesForCurrentUserByDate(2019, 2, 10);
            result.Should().NotBeEmpty();
        }

        [Test]
        public async Task GetProjectTaskEntriesByDate_AddedTimeEntryForGivenDate_ShouldAddOnlyOneEntry()
        {
            this._context.Clients.Add(new Client("testclient1"){Id = 1});
            this._context.Projects.Add(new Project("testProject1", 1, "examplecode"){Id = 1});
            this._context.ProjectTasks.Add(new ProjectTask("testTask1", 1, true){Id = 1});
            _context.SaveChanges();
            
            var firstResult = await _repository.GetProjectTaskEntriesForCurrentUserByDate(2019, 2, 10);
            int numOfEnries = firstResult.ToList().Count;

            var newEntry = new ProjectTaskEntry(_userContext.UserId, 1, 50, new DateTime(2019, 2, 10), "Nasa nota"){Id = 50};
            await _repository.AddTimeEntry(newEntry);

            var result = await _repository.GetProjectTaskEntriesForCurrentUserByDate(2019, 2, 10);
            result.ToList().Count.Should().Be(numOfEnries + 1);
        }
    }
}
