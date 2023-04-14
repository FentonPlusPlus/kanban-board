# Test _1

### 1_GivenARequestToRetrieveATasyById

```csharp
private int _firstTaskId = _testEngine.TaskRepository.Create(new TaskRecord {ClientId = 1, Description = "Ability to LOGIN to the order system.", Status = TaskStatus.BACKLOG, EstimatedDevDays = 5}).Id;
```

### What do we know?

- An int variable called Id is created by accessing the TaskRepository class, calling it’s Create method with an instance of a TaskRecord that takes in 4 parameters in it’s constructor, and then getting the Id of that created instance.
- The firsttaskId has a Description = "Ability to LOGIN to the order system.” which is correctly returned in the first test
- The second test however states

```csharp
var secondTask = _testEngine.TasksController.Get(_secondTaskId);
Assert.That(secondTask.Description, Is.EqualTo("Ability to LOGOUT of the order system."));

Expected string length 38 but was 37. Strings differ at index 14.
Expected: "Ability to LOGOUT of the order system."
But was:  "Ability to LOGIN to the order system."
```

- This implies that it is getting the same result as the first task.

### **What is wrong?**

If we follow the data trail…

```csharp
// the Task Controller calls TaskModel Get(int id)
public TaskModel Get(int id)
        {
            return _taskService.GetById(_session, id);
        }
// which in turn calls TaskService
public TaskModel GetById(Session session, int id)
        {
            var taskRecord = _taskRepository.GetById(id);
            return new TaskModel
            {
                Description = taskRecord.Description,
                Status = taskRecord.Status,
                EstimatedDevDays = taskRecord.EstimatedDevDays,
                Id = taskRecord.Id
            };
        }
// which in turn calls taskRepository.GetById
public TaskRecord GetById(int id)
        {
            using (var connection = _database.Connect())
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var taskRecords = connection.Query<TaskRecord>("SELECT * from task;");
                return taskRecords.First();
            }
        }

// Even though an int id is passed down, the SQL query does not use this and instead is returning the .First() of the taskRecords
```

### **********************How to fix?**********************

- By changing the SQL query to take in a WHERE clause for the id and using a parametised input for the id

```csharp
ar taskRecord = connection.Query<TaskRecord>("SELECT * from task WHERE id = @id;", new { id });
                return taskRecord.First();
```

- You must use a second parameter to define what the parametised variable is
- And you must say you are only expecting 1 result here so you can either use *QueryFirst* or state that on the variable as *taskRecord.First()*

### Notes:

Dapper Queries

[https://zetcode.com/csharp/dapper/](https://zetcode.com/csharp/dapper/)