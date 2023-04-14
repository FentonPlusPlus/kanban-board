# Test _3

### 3_GivenUsersAreAssignedToATask.cs

# ThenTheAssignedUsersAndTheirInitialsAreIncluded

```csharp
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

```

### **What do we know?**

- Two tasks have been created
- 3 users have been assigned a task each
    - Anniyah & Arwel has been assigned firstTaskId
    - Brogan has been assigned secondTaskId
- We are expecting
    - That the tasks information is correct (description & id)
    - That tasks have the correct assigned users that is confirmed through *Username* and *Initials*

### The Problem

- System.NullReferenceException: The Object reference not set to an instance of an object.

### Let’s Single Out the Error

By commeting out each test assert and checking independently

```csharp
[Test]
        public void ThenTheAssignedUsersAndTheirInitialsAreIncluded()
        {
            //Assert.That(_tasks.Tasks[0].Description, Is.EqualTo("Ability to login to the order system."));
            //Assert.That(_tasks.Tasks[0].Id, Is.EqualTo(1));
            //Assert.That(_tasks.Tasks[0].AssignedUsers[0].Username, Is.EqualTo("Anniyah French"));
            //Assert.That(_tasks.Tasks[0].AssignedUsers[1].Username, Is.EqualTo("Arwel Kaye"));

            Assert.That(_tasks.Tasks[0].AssignedUsers[0].Initials, Is.EqualTo("AF"));
            Assert.That(_tasks.Tasks[0].AssignedUsers[1].Initials, Is.EqualTo("AK"));

            //Assert.That(_tasks.Tasks[1].AssignedUsers[0].Username, Is.EqualTo("Brogan Vinson"));
            Assert.That(_tasks.Tasks[1].AssignedUsers[0].Initials, Is.EqualTo("BV"));
        }
```

- We can see that the error is happening with the initials which may help provide more context to the issue

### **What is wrong?**

If we follow the data trail…

```csharp
private GetAllTasksResponse _tasks;
// _tasks is an instance of GetAllTasksResponse

public class GetAllTasksResponse
    {
        public List<TaskModel> Tasks { get; set; }
    }
// Tasks is a List of TaskModel objects
// which in turn has a List of the class TaskAssignedUserModel 
public class TaskModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int EstimatedDevDays { get; set; }
        public List<TaskAssignedUserModel> AssignedUsers { get; set; }
    }

// which in turn has another get Initials function
public class TaskAssignedUserModel
    {
        public string Initials { get; set; }
        public string Username { get; set; }
    }
```

### **********************How to fix?**********************

By changing the get Initials method to now include a similar function as we did for the UserModel

```csharp
public class TaskAssignedUserModel
    {
        public string Initials
        {
            //splits the username based on empty spaces and returns the first character, then joins them together to retrieve the initials
            get
            {
                return string.Join("", Username.Split(' ')
                    .Select(name => name.Substring(0, 1)));
            }
        }
        public string Username { get; set; }
    }
```

### Notes:

- Like the last function, no need to have a setter as we are not storing this information anywhere

# ThenASingleTaskContainsTheAssignedUsers

```csharp
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
```

### **What do we know?**

- Two tasks have been created
- 3 users have been assigned a task each
    - Anniyah & Arwel has been assigned firstTaskId
    - Brogan has been assigned secondTaskId
- **We are expecting**
    - Thatt the firstTask information is correct
        - description
        - id
        - assigned users

### The Problem

- System.NullReferenceException : Object reference not set to an instance of an object.
    - Suggests an object instance has not been created and that that is why it is returning nullRef..
- Isolating the tests shows that the error lies not in the description or Id, **but the assigned users which may help provide context to the issue**

I then decided to follow the api route in succession of what was being called upon tasking from api/tasks, and I came to this code, but I could not see an issue

```csharp
public GetAllTasksResponse GetAll(Session session)
        {
            // we get all the tasks from the tasks table
            var taskRecords = _taskRepository.GetAll();
            // we instantiate a new List for our tasks
            var response = new GetAllTasksResponse{Tasks = new List<TaskModel>()};
            // for each task in task record we 
            foreach (var task in taskRecords)
            {
                // create a taskModel from the task in the loop
                var taskModel = new TaskModel
                {
                    Id = task.Id,
                    Status = task.Status,
                    EstimatedDevDays = task.EstimatedDevDays,
                    Description = task.Description,
                };
                // we say the AssignedUsers will be a new list based on the TaskAssignedUserModel class
                taskModel.AssignedUsers = new List<TaskAssignedUserModel>();
                // we say assigned = a list of users returned based on task.id - is the variable declaration correct here? 
                var assigned = _taskRepository.GetAssignedFor(task.Id);
                // for each assignee we get the user that matched that assigne.UserId and we add this to the assigned users
                foreach (var assignee in assigned)
                {
                    var user = _userRepository.GetAll().First(x => x.Id == assignee.UserId);
                    taskModel.AssignedUsers.Add(new TaskAssignedUserModel { Username = user.Username });
                }
                // we add the created taskModel to the Tasks list
                response.Tasks.Add(taskModel);
            }

            return response;
        }
```

At this point, I am not sure what is wrong, so I am going to double check all SQL queries

- SQL Queries are fine

At this point I took a break and did other tasks 

Upon coming back to it, I could see the issue was actually with the **GetById** function is TaskService

### **What is wrong?**

If we follow the data trail…

```csharp
// The GetById in TaskService does not invoke any function to get who is assigned to the task and return it in the TaskModel
public TaskModel GetById(Session session, int id)
        {
            var taskRecord = _taskRepository.GetById(id);
            

            return new TaskModel
            {
                Description = taskRecord.Description,
                Status = taskRecord.Status,
                EstimatedDevDays = taskRecord.EstimatedDevDays,
                Id = taskRecord.Id,
                
            };
        }

```

### **********************How to fix?**********************

By invoking the taskrepository.GetAssignedFor function that takes in the id passed down, to store the assigned users and then return it in the TaskModel

```csharp
var assignedUsers = new List<TaskAssignedUserModel>();
            var assigned = _taskRepository.GetAssignedFor(id);
            foreach (var assignee in assigned)
            {
                var user = _userRepository.GetAll().First(x => x.Id == assignee.UserId);
                assignedUsers.Add(new TaskAssignedUserModel { Username = user.Username });
            }

return new TaskModel
            {
                Description = taskRecord.Description,
                Status = taskRecord.Status,
                EstimatedDevDays = taskRecord.EstimatedDevDays,
                Id = taskRecord.Id,
                // set the assignedUsers
                AssignedUsers = assignedUsers
            };
```

### Notes:

- Taking a break can often help!