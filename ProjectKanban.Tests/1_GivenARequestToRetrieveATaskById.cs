using NUnit.Framework;
using ProjectKanban.Tasks;

namespace ProjectKanban.Tests
{
    public class _1_GivenARequestToRetrieveATaskById
    {
        private TestEngine _testEngine;
        private int _firstTaskId;
        private int _secondTaskId;

        [OneTimeSetUp]
        public void Setup()
        {
            _testEngine = new TestEngine();
            
            _firstTaskId = _testEngine.TaskRepository.Create(new TaskRecord {ClientId = 1, Description = "Ability to LOGIN to the order system.", Status = TaskStatus.BACKLOG, EstimatedDevDays = 5}).Id;
            _secondTaskId = _testEngine.TaskRepository.Create(new TaskRecord {ClientId = 1, Description = "Ability to LOGOUT of the order system.", Status = TaskStatus.BACKLOG, EstimatedDevDays = 5}).Id;
        }
        
        [Test]
        public void ThenTheFirstTaskIsReturned()
        {
            var firstTask = _testEngine.TasksController.Get(_firstTaskId);
            Assert.That(firstTask.Description, Is.EqualTo("Ability to LOGIN to the order system."));
        }
        
        [Test]
        public void ThenTheSecondTaskIsReturned()
        {
            var secondTask = _testEngine.TasksController.Get(_secondTaskId);
            Assert.That(secondTask.Description, Is.EqualTo("Ability to LOGOUT of the order system."));
        }
    }
}