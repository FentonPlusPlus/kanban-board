using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ProjectKanban.Controllers;
using ProjectKanban.Users;

namespace ProjectKanban.Tasks
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;
        private readonly UserRepository _userRepository;

        public TaskService(TaskRepository taskRepository, UserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public TaskModel GetById(Session session, int id)
        {
            var taskRecord = _taskRepository.GetById(id);
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
        }

        public GetAllTasksResponse GetAll(Session session)
        {
            // we get all the tasks from the tasks table
            var clientId = session.ClientId;
            var taskRecords = _taskRepository.GetAll(clientId);
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
                    //
                    // AssignedUsers = new List<TaskAssignedUserModel>()  -> does this not need to be in the constructor?
                    //
                };
                // we say the AssignedUsers will be a new list based on the TaskAssignedUserModel class
                taskModel.AssignedUsers = new List<TaskAssignedUserModel>();
                // we say assigned = a list of users returned based on task.id
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
            // orders the Tasks based on there status
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

            return response;
        }
    }
}