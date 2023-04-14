using NUnit.Framework;
using ProjectKanban.Controllers;
using ProjectKanban.Tasks;

namespace ProjectKanban.Tests
{
    public sealed class _4_GivenARequestToRetrieveAllTheTasks
    {
        private TestEngine _testEngine;
        private GetAllTasksResponse _tasks;

        [OneTimeSetUp]
        public void Setup()
        {
            _testEngine = new TestEngine();
            _testEngine.TaskRepository.Create(new TaskRecord {ClientId = 1, Description = "When logging on as a client should only see tasks for that client.", Status = TaskStatus.BACKLOG });
            _testEngine.TaskRepository.Create(new TaskRecord {ClientId = 1, Description = "Ability to create an account", Status = TaskStatus.DONE });
            _testEngine.TaskRepository.Create(new TaskRecord {ClientId = 1, Description = "Ability to view a task", Status = TaskStatus.IN_SIGNOFF });
            _testEngine.TaskRepository.Create(new TaskRecord {ClientId = 1, Description = "Ability to create a client in the system", Status = TaskStatus.IN_PROGRESS });

            _tasks = _testEngine.TasksController.GetAllTasksResponse();
        }

        [Test]
        public void ThenTheDoneTasksAreReturnedFirst()
        {
            Assert.That(_tasks.Tasks[0].Description, Is.EqualTo("Ability to create an account"));
            Assert.That(_tasks.Tasks[0].Status, Is.EqualTo(TaskStatus.DONE));
        }
        
        [Test]
        public void ThenTheSignOffTasksAreReturnedSecond()
        {
            Assert.That(_tasks.Tasks[1].Description, Is.EqualTo("Ability to view a task"));
            Assert.That(_tasks.Tasks[1].Status, Is.EqualTo(TaskStatus.IN_SIGNOFF));
        }
        
        [Test]
        public void ThenTheInProgressTasksAreReturnedThird()
        {
            Assert.That(_tasks.Tasks[2].Description, Is.EqualTo("Ability to create a client in the system"));
            Assert.That(_tasks.Tasks[2].Status, Is.EqualTo(TaskStatus.IN_PROGRESS));
        }
        
        [Test]
        public void ThenTheBacklogTasksAreReturnedLast()
        {
            Assert.That(_tasks.Tasks[3].Description, Is.EqualTo("When logging on as a client should only see tasks for that client."));
            Assert.That(_tasks.Tasks[3].Status, Is.EqualTo(TaskStatus.BACKLOG));
        }
    }
}