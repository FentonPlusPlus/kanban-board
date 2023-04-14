using NUnit.Framework;
using ProjectKanban.Controllers;
using ProjectKanban.Tasks;

namespace ProjectKanban.Tests
{
    public sealed class _2_GivenARequestToRetrieveAListOfUsers
    {
        private TestEngine _testEngine;
        private AllUsersResponse _usersResponse;

        [OneTimeSetUp]
        public void Setup()
        {
            _testEngine = new TestEngine();
          
            _usersResponse = _testEngine.UsersController.GetAll();
        }

        [Test]
        public void ThenTheUserListIsReturnedAlphabetically()
        {
            Assert.That(_usersResponse.Users[0].Username, Is.EqualTo("Anniyah French"));
            Assert.That(_usersResponse.Users[1].Username, Is.EqualTo("Arwel Kaye"));
            Assert.That(_usersResponse.Users[2].Username, Is.EqualTo("Brogan Vinson"));
            Assert.That(_usersResponse.Users[3].Username, Is.EqualTo("Dustin Schaefer"));
            Assert.That(_usersResponse.Users[4].Username, Is.EqualTo("Irving Weston"));
            Assert.That(_usersResponse.Users[5].Username, Is.EqualTo("Karla Ellis"));
            Assert.That(_usersResponse.Users[6].Username, Is.EqualTo("Kieran Edge"));
            Assert.That(_usersResponse.Users[7].Username, Is.EqualTo("Shayna Ortega"));
            Assert.That(_usersResponse.Users[8].Username, Is.EqualTo("Zaid Thorne"));
        }

        [Test]
        public void ThenTheInitialsAreGeneratedForEachUser()
        {
            Assert.That(_usersResponse.Users[0].Initials, Is.EqualTo("AF"));
        }
    }
}