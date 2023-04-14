using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectKanban.Data;
using ProjectKanban.Tasks;
using ProjectKanban.Users;

namespace ProjectKanban.Controllers
{
    [Route("api/tasks")]
    public  class TasksController : Controller
    {
        private readonly Session _session;
        private readonly UserRepository _userRepository;
        private TaskService _taskService;

        public TasksController(TaskRepository taskRepository, Session session, UserRepository userRepository)
        {
            _session = session;
            _userRepository = userRepository;
            _taskService = new TaskService(taskRepository, _userRepository);
        }
        
        [HttpGet("{id}")]
        public TaskModel Get(int id)
        {
            return _taskService.GetById(_session, id);
        }

        public GetAllTasksResponse GetAllTasksResponse()
        {
            return _taskService.GetAll(_session);
        }
    }

    public class TaskModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int EstimatedDevDays { get; set; }
        public List<TaskAssignedUserModel> AssignedUsers { get; set; }
    }

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

    public class GetAllTasksResponse
    {
        public List<TaskModel> Tasks { get; set; }
    }
}