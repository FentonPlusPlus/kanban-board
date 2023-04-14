# Test _5

# 5_GivenARequestToRetrieveTasksAsAClient

```csharp
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
```

### **What do we know?**

- 2 Clients have been created in the ClientsRepository
- 2 Users have been created in the UserRepository
- They have had tasks created in the TaskRepository and assigned to them correctly

### What do we not know?

- What a Count is
    - In built function on a list where you can retrieve the length of said list

### The Problem

We’re expecting to only get a Count of 1 back for when either we retrieve only Volkswagen Tasks or BMW tasks, yet we get 2

- suggests a duplication error, or we’re retrieving the other users tasks too which is more likely.
- do we need to pass down the client id into the SQL query to only retrieve relevant client tasks?

### **What is wrong?**

If we follow the data trail…

```csharp
// Get all will retrieve every task from the task table, regardless of which client they belong to. Test 5 implies that only a logged in client should see there relevant tasks
public List<TaskRecord> GetAll()
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var taskRecords = connection.Query<TaskRecord>("SELECT * from task;").ToList();
                return taskRecords;
            }
        }
```

### Ideal/Dumb Solution

To have the Session read the logged in client id and make sure the TasksController is only retrieiving tasks based on that ClientId

### So Where can we get Client Id from?

```csharp
// Session should contain ClientId, as this is representative of the data table
namespace ProjectKanban.Users
{
    public sealed class Session
    {
        public string Username { get; set; }
        public int UserId { get; set; }
        // public int ClientId { get; set; }
    }
}
```

### **********************How to fix?**********************

By passing down the clientId from session into the SQL query

```csharp
public List<TaskRecord> GetAll(int id)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var taskRecords = connection.Query<TaskRecord>("SELECT * from task WHERE client_id = @Id;", new { Id = id }).ToList();
                return taskRecords;
            }
        }

// This must be passed down at TaskService instead of TaskController because we cannot edit how the test invokes TaskControllers.getAllTasksResponse()

public GetAllTasksResponse GetAll(Session session)
        {
            // we get all the tasks from the tasks table
            var clientId = session.ClientId;
						var taskRecords = _taskRepository.GetAll(clientId);
```

### Notes:

- The SQL query needed the anonymous typing to work.
    - Have not tried this with just *new { id }*