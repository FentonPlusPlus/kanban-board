using System.Linq;
using NUnit.Framework;
using ProjectKanban.Controllers;
using ProjectKanban.Tasks;

namespace ProjectKanban.Tests
{
    public sealed class _3_GivenUsersAreAssignedToATask
    {
        private TestEngine _testEngine;
        private GetAllTasksResponse _tasks;
        private TaskModel _firstTask;

        [OneTimeSetUp]
        public void Setup()
        {
            _testEngine = new TestEngine();
            
            var firstTaskId = _testEngine.TaskRepository.Create(new TaskRecord {ClientId = 1, Description = "Ability to login to the order system.", Status = TaskStatus.BACKLOG, EstimatedDevDays = 5}).Id;
            var secondTaskId = _testEngine.TaskRepository.Create(new TaskRecord {ClientId = 1, Description = "Ability to logout of the order system.", Status = TaskStatus.BACKLOG, EstimatedDevDays = 5}).Id;
            
            var anniyahFrenchUserId = 1;
            var arwelKayeUserId = 2;
            var broganVinsonId3 = 3;
            
            _testEngine.TaskRepository.CreateAssigned(new TaskAssignedRecord{TaskId = firstTaskId, UserId = anniyahFrenchUserId});
            _testEngine.TaskRepository.CreateAssigned(new TaskAssignedRecord{TaskId = firstTaskId, UserId = arwelKayeUserId});
            _testEngine.TaskRepository.CreateAssigned(new TaskAssignedRecord{TaskId = secondTaskId, UserId = broganVinsonId3});
            
            _firstTask = _testEngine.TasksController.Get(1);
            _tasks = _testEngine.TasksController.GetAllTasksResponse();
        }

        [Test]
        public void ThenTheAssignedUsersAndTheirInitialsAreIncluded()
        {
            Assert.That(_tasks.Tasks[0].Description, Is.EqualTo("Ability to login to the order system."));
            Assert.That(_tasks.Tasks[0].Id, Is.EqualTo(1));
            Assert.That(_tasks.Tasks[0].AssignedUsers[0].Username, Is.EqualTo("Anniyah French"));
            Assert.That(_tasks.Tasks[0].AssignedUsers[1].Username, Is.EqualTo("Arwel Kaye"));

            Assert.That(_tasks.Tasks[0].AssignedUsers[0].Initials, Is.EqualTo("AF"));
            Assert.That(_tasks.Tasks[0].AssignedUsers[1].Initials, Is.EqualTo("AK"));

            Assert.That(_tasks.Tasks[1].AssignedUsers[0].Username, Is.EqualTo("Brogan Vinson"));
            Assert.That(_tasks.Tasks[1].AssignedUsers[0].Initials, Is.EqualTo("BV"));
        }

        [Test]
        public void ThenASingleTaskContainsTheAssignedUsers()
        {
            Assert.That(_firstTask.Description, Is.EqualTo("Ability to login to the order system."));
            Assert.That(_firstTask.Id, Is.EqualTo(1));
            Assert.That(_firstTask.AssignedUsers[0].Username, Is.EqualTo("Anniyah French"));
            Assert.That(_firstTask.AssignedUsers[1].Username, Is.EqualTo("Arwel Kaye"));

            Assert.That(_firstTask.AssignedUsers[0].Initials, Is.EqualTo("AF"));
            Assert.That(_firstTask.AssignedUsers[1].Initials, Is.EqualTo("AK"));
        }
    }
}