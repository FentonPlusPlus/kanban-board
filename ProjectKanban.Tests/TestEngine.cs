using ProjectKanban.Controllers;
using ProjectKanban.Data;
using ProjectKanban.Tasks;
using ProjectKanban.Users;

namespace ProjectKanban.Tests
{
    public sealed class TestEngine
    {
        public TasksController TasksController;
        public UsersController UsersController;
        
        private SqliteMemoryDatabase _database;
        public TaskRepository TaskRepository;
        public ClientsRepository ClientsRepository;
        public UserRepository UserRepository;

        public TestEngine() : this("Zaid Thorne", "123")
        {
        }


        public TestEngine(string username, string password)
        {
            _database = new SqliteMemoryDatabase();
            TaskRepository = new TaskRepository(_database);
            UserRepository = new UserRepository(_database);
            UsersController = new UsersController(UserRepository);
            ClientsRepository = new ClientsRepository(_database);
            
            ClientsRepository.Create(new ClientRecord{Name = "None"});

            UserRepository.Create(new UserRecord {ClientId = 1, Username = "Anniyah French"});
            UserRepository.Create(new UserRecord {ClientId = 1, Username = "Arwel Kaye"});
            UserRepository.Create(new UserRecord {ClientId = 1, Username = "Brogan Vinson"});
            UserRepository.Create(new UserRecord {ClientId = 1, Username = "Dustin Schaefer"});
            UserRepository.Create(new UserRecord {ClientId = 1, Username = "Karla Ellis"});
            UserRepository.Create(new UserRecord {ClientId = 1, Username = "Irving Weston"});
            UserRepository.Create(new UserRecord {ClientId = 1, Username = "Kieran Edge"});
            UserRepository.Create(new UserRecord {ClientId = 1, Username = "Shayna Ortega"});
            UserRepository.Create(new UserRecord {ClientId = 1, Username = "Zaid Thorne", Password = "123"});

           Login(username, password);

        }

        public void Login(string username, string password)
        {
            var session = UsersController.Login(new LoginRequest
            {
                Username = username,
                Password = password
            });
            TasksController = new TasksController(TaskRepository, session, UserRepository);
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}