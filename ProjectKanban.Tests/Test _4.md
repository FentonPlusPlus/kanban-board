# Test _4

# 4_GivenARequestToRetrieveAllTheTasks

```csharp
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
```

### **What do we know?**

- 4 tasks have been created without estimated_dev_days
- We are expecting that it is returning the tasks in the appropriate order
    - DONE → SIGNOFF → IN PROGRESS → BACKLOG

### The Problem

We need a way to correctly organise these tasks when they are retrieved from the database

### **What is wrong?**

If we follow the data trail…

```csharp
// when _tasks is declared in the test
_tasks = _testEngine.TasksController.GetAllTasksResponse();
// it calls on this function is Tasks Controller
public GetAllTasksResponse GetAllTasksResponse()
        {
            return _taskService.GetAll(_session);
        }
// which in turn calls the GetAll from the tasksService class.
```

- Currently there is nothing ordering these tasks so we need the function GetAllTasksResponse to do so.

### **********************How to fix?**********************

By creating a switch statement and using the OrderBy method to organise the List

```csharp
response.Tasks = response.Tasks.OrderBy(task =>
            {
                switch (task.Status)
                {
                    case TaskStatus.DONE:
                        return 0;
                    case TaskStatus.IN_SIGNOFF:
                        return 1;
                    case TaskStatus.IN_PROGRESS:
                        return 2;
                    case TaskStatus.BACKLOG:
                        return 3;
                    default:
                        return 4;
                }
            }).ToList();
```

### Notes:

- I think this is best to do in the TaskService as this is where a lot of the functionality happens in organising data retrieved from the database