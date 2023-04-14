using NUnit.Framework;
using ProjectKanban.Controllers;
using ProjectKanban.Tasks;

namespace ProjectKanban.Tests
{
    public sealed class _5_GivenARequestToRetrieveTasksAsAClient
    {
        private TestEngine _testEngine;

        [OneTimeSetUp]
        public void Setup()
        {
            _testEngine = new TestEngine();
            _testEngine.ClientsRepository.Create(new ClientRecord{Name = "Volkswagen"});
            _testEngine.ClientsRepository.Create(new ClientRecord{Name = "BMW"});
            _testEngine.TaskRepository.Create(new TaskRecord{Description = "Volkswagen task", ClientId = 2});
            _testEngine.TaskRepository.Create(new TaskRecord{Description = "BMW task", ClientId = 3});
            
            _testEngine.UserRepository.Create(new UserRecord{Username = "vw", Password = "vw", ClientId = 2});
            _testEngine.UserRepository.Create(new UserRecord{Username = "bmw", Password = "bmw", ClientId = 3});
        }

        [Test]
        public void WhenVolkswagenRetrievesTasksThenOnlyVolkswagenTasksAreReturned()
        {
          _testEngine.Login("vw", "vw");
          var tasks = _testEngine.TasksController.GetAllTasksResponse();
          Assert.That(tasks.Tasks.Count, Is.EqualTo(1));
          Assert.That(tasks.Tasks[0].Description, Is.EqualTo("Volkswagen task"));
        }
        
        [Test]
        public void WhenBmwRetrievesTasksThenOnlyBmwTasksAreReturned()
        {
            _testEngine.Login("bmw", "bmw");
            var tasks = _testEngine.TasksController.GetAllTasksResponse();
            Assert.That(tasks.Tasks.Count, Is.EqualTo(1));
            Assert.That(tasks.Tasks[0].Description, Is.EqualTo("BMW task"));
        }
    }
}